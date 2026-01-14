# VPM Repository Setup Guide / VPMリポジトリセットアップガイド

[English](#english) | [日本語](#japanese)

---

## English

### Setup Instructions

This repository is configured as a VPM (VRChat Package Manager) compatible repository.

#### For Repository Maintainers

1. **Enable GitHub Pages**
   - Go to repository Settings > Pages
   - Set Source to "Deploy from a branch"
   - Select branch: `gh-pages`
   - Click Save

2. **Create a Release**
   - Go to Releases > Create a new release
   - Tag version: `v1.2.5` (or your version)
   - Add release notes
   - The GitHub Actions workflow will automatically:
     - Generate `index.json`
     - Deploy to GitHub Pages
     - Make the package available via VPM

3. **Verify Setup**
   - After the workflow completes, check:
     - `https://[username].github.io/SimpleFolderIcon/index.json`
   - This URL should return the VPM repository listing

#### For Users

Add this VPM repository to your VRChat Creator Companion:

```
https://raw.githubusercontent.com/S0iRu/SimpleFolderIcon/master/index.json
```

1. Open VCC (VRChat Creator Companion)
2. Go to Settings > Packages > Add Repository
3. Paste the URL above
4. The "Simple Folder Icon" package will appear in your project's package list

---

## Japanese

### セットアップ手順

このリポジトリはVPM（VRChat Package Manager）対応リポジトリとして設定されています。

#### リポジトリ管理者向け

1. **GitHub Pagesを有効化**
   - リポジトリのSettings > Pagesに移動
   - Sourceを "Deploy from a branch" に設定
   - ブランチを `gh-pages` に選択
   - 保存をクリック

2. **リリースを作成**
   - Releases > Create a new releaseに移動
   - タグバージョン: `v1.2.5`（またはあなたのバージョン）
   - リリースノートを追加
   - GitHub Actionsワークフローが自動的に:
     - `index.json`を生成
     - GitHub Pagesにデプロイ
     - VPM経由でパッケージを利用可能にする

3. **セットアップの確認**
   - ワークフロー完了後、以下を確認:
     - `https://[username].github.io/SimpleFolderIcon/index.json`
   - このURLがVPMリポジトリリストを返すはずです

#### ユーザー向け

このVPMリポジトリをVRChat Creator Companionに追加:

```
https://raw.githubusercontent.com/S0iRu/SimpleFolderIcon/master/index.json
```

1. VCC（VRChat Creator Companion）を開く
2. Settings > Packages > Add Repositoryに移動
3. 上記のURLを貼り付け
4. プロジェクトのパッケージリストに「Simple Folder Icon」パッケージが表示されます

---

## Technical Details / 技術詳細

### Files Added / 追加されたファイル

- `index.json` - VPM repository manifest / VPMリポジトリマニフェスト
- `.github/workflows/build-listing.yml` - Automated build workflow / 自動ビルドワークフロー
- Updated `package.json` with VPM-specific fields / VPM固有フィールドを追加したpackage.json

### Package Structure / パッケージ構造

```
Packages/
  └─ com.S0iRu.simple-folder-icon/
     ├─ package.json          # Unity Package manifest with VPM fields
     ├─ Editor/               # Editor scripts
     └─ Icons/                # Folder icons
```

### Updating the Package / パッケージの更新

1. Update version in `Packages/com.S0iRu.simple-folder-icon/package.json`
2. Create a new release with matching tag (e.g., `v1.2.6`)
3. GitHub Actions will automatically update the VPM listing

1. `Packages/com.S0iRu.simple-folder-icon/package.json`のバージョンを更新
2. 一致するタグで新しいリリースを作成（例：`v1.2.6`）
3. GitHub Actionsが自動的にVPMリストを更新します
