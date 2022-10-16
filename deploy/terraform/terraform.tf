provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = lower("${local.project-prefix}_${local.project-shortname}_demo")
  location = var.location
}

resource "azurerm_storage_account" "storage" {
  name                      = lower("${local.project-prefix}storage")
  resource_group_name       = azurerm_resource_group.rg.name
  location                  = azurerm_resource_group.rg.location
  account_tier              = "Standard"
  account_kind              = "StorageV2"
  account_replication_type  = "LRS"
  enable_https_traffic_only = true
  
  static_website {
   index_document = "index.html"
    error_404_document = "index.html"
  }
}

resource "azurerm_service_plan" "appserviceplan" {
  name                = lower("${local.project-prefix}-app-plan")
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "api" {
  name                       = lower("${local.project-prefix}-api")
  resource_group_name        = azurerm_resource_group.rg.name
  location                   = azurerm_resource_group.rg.location
  service_plan_id            = azurerm_service_plan.appserviceplan.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key
  tags                        = {}
  functions_extension_version = "~4"
  


  app_settings = {
    "WEBSITE_RUN_FROM_PACKAGE"       = "1"
  }


  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    cors {
        allowed_origins = ["*"]
    }
  }
}

variable "project-prefix" {
  description = "Prefix, should contain unique prefix and short description separated by underscore character (i.e. demo214_test)."
  validation {
    condition     = length(split("_", var.project-prefix)) >= 2
    error_message = "Prefix, should contain unique prefix and short description separated by underscore character (i.e. demo214_test)."
  }
}

variable "location" {
  description = "The Azure Region in which all resources in this example should be created."
  default = "EastUS"
}

locals {
  project-prefix            = lower(split("_", var.project-prefix)[0])
  project-shortname         = lower(split("_", var.project-prefix)[1])
}

output web-app-settings {
    sensitive =false
    value = <<OUT
{
  "ApiSettings": {
    "BaseUrl": "https://${azurerm_linux_function_app.api.name}.azurewebsites.net"
  }
}
OUT
}

output web-app-address {
  sensitive =false
  value = azurerm_storage_account.storage.primary_web_endpoint 
}