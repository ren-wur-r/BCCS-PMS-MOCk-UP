#!/bin/bash

MOCK_ENV_FILE=".env.development.local"
BACKUP_FILE=".env.development.local.backup"

show_help() {
    echo "用法: ./switch-mode.sh [選項]"
    echo ""
    echo "選項:"
    echo "  mock      切換到 Mock API 模式（在家開發）"
    echo "  real      切換到真實 API 模式（在公司開發）"
    echo "  status    查看目前模式"
    echo "  help      顯示此說明"
    echo ""
    echo "範例:"
    echo "  ./switch-mode.sh mock     # 啟用 Mock 模式"
    echo "  ./switch-mode.sh real     # 啟用真實 API 模式"
    echo "  ./switch-mode.sh status   # 查看目前狀態"
}

show_status() {
    if [ -f "$MOCK_ENV_FILE" ]; then
        USE_MOCK=$(grep "VITE_USE_MOCK_DATA" "$MOCK_ENV_FILE" | cut -d '=' -f2)
        if [ "$USE_MOCK" = "true" ]; then
            echo "目前模式: Mock API 模式（在家開發）"
            echo "API 端點: http://localhost:5011/"
        else
            echo "目前模式: 真實 API 模式（連接後端）"
            API_URL=$(grep "VITE_API_BASE_URL" "$MOCK_ENV_FILE" | cut -d '=' -f2)
            echo "API 端點: $API_URL"
        fi
    else
        echo "目前模式: 預設模式（使用 .env.development）"
        echo "API 端點: http://192.168.3.236:8100/ProjectManagerWebApi/"
    fi
}

switch_to_mock() {
    if [ -f "$MOCK_ENV_FILE" ]; then
        cp "$MOCK_ENV_FILE" "$BACKUP_FILE"
    fi

    cat > "$MOCK_ENV_FILE" << EOF
VITE_API_BASE_URL=http://localhost:5011/
VITE_BASE_PATH=/
VITE_USE_MOCK_DATA=true
EOF

    echo "已切換到 Mock API 模式"
    echo "請重新啟動開發伺服器（npm run dev）"
}

switch_to_real() {
    if [ -f "$MOCK_ENV_FILE" ]; then
        if [ -f "$BACKUP_FILE" ]; then
            echo "備份檔案已存在於 $BACKUP_FILE"
        else
            cp "$MOCK_ENV_FILE" "$BACKUP_FILE"
            echo "已備份至 $BACKUP_FILE"
        fi
        rm "$MOCK_ENV_FILE"
        echo "已切換到真實 API 模式"
        echo "將使用 .env.development 的設定"
        echo "請重新啟動開發伺服器（npm run dev）"
    else
        echo "目前已經是真實 API 模式"
    fi
}

case "$1" in
    mock)
        switch_to_mock
        ;;
    real)
        switch_to_real
        ;;
    status)
        show_status
        ;;
    help|--help|-h|"")
        show_help
        ;;
    *)
        echo "錯誤: 不認識的選項 '$1'"
        echo ""
        show_help
        exit 1
        ;;
esac
