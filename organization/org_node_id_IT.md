# 創新服務研發處

## 1. 組織節點定義（Org Node）

- org_node_id: RND
- org_node_name: 創新服務研發處
- org_node_dept: 監控部(MON)、鑑識部(FORE)
- parent_org_node_id: CEO
- org_path: 總經理 / 創新服務研發處

## 2. RND 服務目錄（Service Catalog）

說明：

- service_id：服務唯一 ID（穩定、可被系統引用）
- product_major：服務項目（英文系統識別）
- product_minor：產品
- product_description：產品說明（可選，若無則不顯示）
- owner_org_node_id：該服務歸屬哪個組織節點
- tags：可選，用於檢索/分類

### 2.1 創新服務研發處（RND）服務清單

- service_id: RND-CLOUD-MSSP-RAPIXUS
  product_major: cloud_security_operations
  product_name: MSSP By Rapixus VANS
  product_description: MSSP By Rapixus
  owner_org_node_id: RND
  tags: [mssp, rapixus]

- service_id: RND-CLOUD-BCCS-VANS
  product_major: cloud_security_operations
  product_name: MSSP By Rapixus VANS
  product_description: BCCS 雲端工具
  owner_org_node_id: RND
  tags: [mssp, bccs]

- service_id: RND-CLOUD-ADV-MDR-EDR
  product_major: cloud_security_operations
  product_name: advMDR主動入侵威脅鑑識管理服務(含T5 EDR)
  product_description: BCCS 雲端服務
  owner_org_node_id: RND
  tags: [mdr, edr, bccs]

- service_id: RND-CLOUD-ADV-MDR-NO-EDR
  product_major: cloud_security_operations
  product_name: advMDR主動入侵威脅鑑識管理服務(不含EDR授權)
  product_description: BCCS 雲端服務
  owner_org_node_id: RND
  tags: [mdr, bccs]

- service_id: RND-CLOUD-SOC-LOW
  product_major: cloud_security_operations
  product_name: SOC監控服務低流量
  product_description: BCCS 雲端服務
  owner_org_node_id: RND
  tags: [soc, low]

- service_id: RND-CLOUD-SOC-MID
  product_major: cloud_security_operations
  product_name: SOC監控服務中流量
  product_description: BCCS 雲端服務
  owner_org_node_id: RND
  tags: [soc, mid]

- service_id: RND-CLOUD-SOC-HIGH
  product_major: cloud_security_operations
  product_name: SOC監控服務高流量
  product_description: BCCS 雲端服務
  owner_org_node_id: RND
  tags: [soc, high]

- service_id: RND-CLOUD-MSIEM-5X8
  product_major: cloud_security_operations
  product_name: mSIEM 5x8監控告警與回應服務(不含SIEM授權)
  product_description: BCCS 雲端服務
  owner_org_node_id: RND
  tags: [msiem, 5x8, bccs]
