# 風險稽核處

## 1. 組織節點定義（Org Node）

- org_node_id: RISK
- org_node_name: 風險稽核處
- parent_org_node_id: CEO
- org_path: 總經理 / 風險稽核處

## 2. RISK 服務目錄（Service Catalog）

說明：

- service_id：服務唯一 ID（穩定、可被系統引用）
- product_major：服務項目（英文系統識別）
- product_minor：產品
- product_description：產品說明（可選，若無則不顯示）
- owner_org_node_id：該服務歸屬哪個組織節點
- tags：可選，用於檢索/分類

### 2.1 風險稽核處（RISK）服務清單

- service_id: RISK-VA-SYS-REMOTE
  product_major: risk_detection
  product_name: 系統弱點掃描(VA)-遠端服務
  owner_org_node_id: RISK
  tags: [VA, remote, system]

- service_id: RISK-VA-SYS-ONSITE
  product_major: risk_detection
  product_name: 系統弱點掃描(VA)-到場服務
  owner_org_node_id: RISK
  tags: [VA, onsite, system]

- service_id: RISK-VA-WEB-REMOTE
  product_major: risk_detection
  product_name: 網站弱點掃描(WEB VA)-遠端服務
  owner_org_node_id: RISK
  tags: [WebVA, remote, web]

- service_id: RISK-VA-WEB-ONSITE
  product_major: risk_detection
  product_name: 網站弱點掃描(WEB VA)-到場服務
  owner_org_node_id: RISK
  tags: [WebVA, onsite, web]

- service_id: RISK-PT-REMOTE
  product_major: risk_detection
  product_name: 滲透測試(PT)-遠端服務
  owner_org_node_id: RISK
  tags: [PT, remote]

- service_id: RISK-PT-ONSITE
  product_major: risk_detection
  product_name: 滲透測試(PT)-到場服務
  owner_org_node_id: RISK
  tags: [PT, onsite]

- service_id: RISK-CODE-REVIEW
  product_major: risk_detection
  product_name: 源碼檢測服務
  owner_org_node_id: RISK
  tags: [code, review]

- service_id: RISK-SE-EMAIL-100
  product_major: risk_detection
  product_name: 100 個帳號（1~100）
  owner_org_node_id: RISK
  tags: [social-engineering, email, 1-100]

- service_id: RISK-SE-EMAIL-200
  product_major: risk_detection
  product_name: 200 個帳號（101~200）
  owner_org_node_id: RISK
  tags: [social-engineering, email, 101-200]

- service_id: RISK-SE-EMAIL-500
  product_major: risk_detection
  product_name: 500 個帳號（201~500）
  owner_org_node_id: RISK
  tags: [social-engineering, email, 201-500]
