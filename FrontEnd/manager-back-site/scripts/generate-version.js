import { execSync } from "child_process";
import fs from "fs";
import path from "path";

/**
 * 取得最新 git tag
 */
function getGitTag() {
  try {
    return execSync("git describe --tags --abbrev=0")
      .toString()
      .trim();
  } catch {
    return "unknown";
  }
}

/**
 * 產生版本資訊檔案
 */
function generateVersionFile() {
  const versionInfo = {
    version: getGitTag(),
  };

  const outputPath = path.resolve("public/version.json");

  fs.writeFileSync(outputPath, JSON.stringify(versionInfo, null, 2));

  console.log("version.json 已產生", versionInfo);
}

generateVersionFile();
