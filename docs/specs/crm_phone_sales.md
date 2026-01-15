# Spec - CRM Phone Sales (Customer)

## Overview
Manages phone sales customer records separate from activity-based telemarketing lists.

## Frontend Logic
- Routes:
  - `/crm/phone-sales/customer` (list/add/detail)
- Views: `FrontEnd/manager-back-site/src/views/crm/phone-sales/customer/*`.

## Backend Logic
- Controller: `MbsCrmPhoneSalesController`.
- Endpoints (from code):
  - `POST /api/MbsCrmPhoneSales/GetManyCustomer`
  - `POST /api/MbsCrmPhoneSales/GetCustomer`
  - `POST /api/MbsCrmPhoneSales/AddCustomer`
  - `POST /api/MbsCrmPhoneSales/UpdateCustomer`
  - `POST /api/MbsCrmPhoneSales/DeleteCustomer`
  - `POST /api/MbsCrmPhoneSales/UpdateCustomerOrder`

