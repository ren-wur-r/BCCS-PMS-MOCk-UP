# 管理顧問處

## 1. 組織節點定義（Org Node）

- org_node_id: CONS
- org_node_name: 管理顧問處
- parent_org_node_id: CEO
- org_path: 總經理 / 管理顧問處

## 2. CONS 服務目錄（Service Catalog）

說明：

- service_id：服務唯一 ID（穩定、可被系統引用）
- product_major：服務項目（英文系統識別）
- product_minor：產品
- product_description：產品說明（可選，若無則不顯示）
- consultant：顧問（可選，若無則不顯示）
- owner_org_node_id：該服務歸屬哪個組織節點
- tags：可選，用於檢索/分類

### 2.1 管理顧問處（CONS）服務清單

- service_id: CONS-RISK-AUDIT-ADVISORY
  product_major: consulting_services
  product_name: 資安風險管理與稽核機制輔導
  product_description: 稽核顧問輔導服務
  consultant: 呂瑋原
  owner_org_node_id: CONS
  tags: [risk, audit, advisory]

- service_id: CONS-ISMS-NEW
  product_major: consulting_services
  product_name: ISMS資訊安全管理
  product_description: 新評
  consultant: 呂瑋原
  owner_org_node_id: CONS
  tags: [isms]

- service_id: CONS-ISMS-NOCERT
  product_major: consulting_services
  product_name: ISMS資訊安全管理
  product_description: 不含驗證
  owner_org_node_id: CONS
  tags: [isms]

- service_id: CONS-PIMS-REVIEW
  product_major: consulting_services
  product_name: PIMS個資保護管理
  product_description: 複評
  consultant: 孫加榮
  owner_org_node_id: CONS
  tags: [pims]
