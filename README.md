# dotnet-feeder

Dotnet Feeder is a collection of handy tools to do stuff with markdown files. It's all build in .NET 6 and available as a [dotnet-tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools). Some features can also be used as a Github Action.

[![github issues][badge_issues]][link_issues]
[![License][badge_license]](link_license)
[![Support me on Github][badge_sponsor]][link_sponsor]

## Feed command

Use this command to read a json post feed and to replace the text between the tags with a list of posts.
You can use this command to automatically update any markdown with the latest posts.
I developed this to update my [Github profile readme](https://docs.github.com/en/account-and-profile/setting-up-and-managing-your-github-profile/customizing-your-profile/managing-your-profile-readme) every day.

Be sure to add at least one file that has both the start tag `<!-- start {tag} -->` and the end tag `<!-- end {tag} -->`.

```plain
Feeder v1.0.0

USAGE
  dotnet-feeder feed <url> <files...> [options]

DESCRIPTION
  Read a feed and write markdown

PARAMETERS
* url               Url of the feed to parse 
* files             File(s) to write posts to 

OPTIONS
  --count           Number of items to use Default: "10".
  --tag             Tag to look for, <!-- start {tag} --> / <!-- end {tag} --> Default: "posts".
  --template        Item template when writing markdown Default: "- [{title}]({url})".
  -h|--help         Shows help text. 
```

### Feed command - Github actions

To use this action for your own README file, do the following:

1. Add a header to your README (posts need their own header right?) `## Recent posts`
2. Skip a line and add `<!-- start posts --><!-- end posts -->` to it.
3. Create a workflow like the one below
4. Change the feed parameter (or leave it if you want to promote my posts)

```yaml
on:
  schedule:
    - cron: '45 6 * * *'
  workflow_dispatch:

jobs:
  update-readme:
    runs-on: ubuntu-latest
    permissions:
      contents: write

    steps:
      - uses: actions/checkout@v3
      - name: Dotnet-feeder
        uses: svrooij/dotnet-feeder@main
        with:
          feed: https://svrooij.io/feed.json
          files: ./README.md
      - uses: stefanzweifel/git-auto-commit-action@v4
        with:
          commit_message: Posts refreshed
          file_pattern: README.md
```

This workflow will trigger daily at 6:45 and can be triggered from the actions screen (or other repositories :wink:)

## Sample posts from my blog

By running `dotnet-feeder feed https://svrooij.io/feed.json ./README.md --count 5 --tag s_posts` your can generate the section below.

<!-- start s_posts -->
- [Protect against certificate extraction - encryption](https://svrooij.io/2022/06/01/certificate-extraction-encryption/) tagged: Security, Encryption
- [Protect against certificate extraction - Client credentials](https://svrooij.io/2022/05/27/certificate-extraction-client-credentials/) tagged: Security, Azure AD
- [Extract all Azure AD admin accounts](https://svrooij.io/2022/05/17/extract-azure-admins/) tagged: Powershell, Azure AD, Data Extraction
- [Extract all users with powershell and what you should do about it](https://svrooij.io/2022/05/16/extract-all-users-with-powershell/) tagged: Powershell, Azure AD, Data Extraction
- [Deploy to Azure Static Web App with only the name](https://svrooij.io/2022/05/05/deploy-static-web-app-without-token/) tagged: Static Web App, Azure CLI
- [Access Azure AD protected API with managed Identity](https://svrooij.io/2022/04/21/access-api-with-managed-identity/) tagged: Graph API, Azure AD, Managed Identity
<!-- end s_posts -->

## Developer stuff

[![github issues][badge_issues]][link_issues]
[![Support me on Github][badge_sponsor]][link_sponsor]

This repository contains of the following files:

| Path | Name | Description |
|------|------|-------------|
| `action.yml` | Action configuration | Action file for this action to be executable from other repositories. |
| `.github/workflows/build.yml` | Build workflow | Workflow for running the tests and creating the [docker image](https://github.com/svrooij/dotnet-feeder/pkgs/container/dotnet-feeder) |
| `.github/workflows/refresh.yml` | Sample workflow | Sample workflow to refresh the posts in the readme. |
| `src/*` | dotnet-feeder source | The actual .NET application running this stuff, using [CliFx](https://github.com/Tyrrrz/CliFx) (it's awesome) |
| `tests/*` | tests | Some tests that are mandatory in the build/release pipeline |

If you want to create your own github action in .NET code, this repository is a great place to start. Let me know what you think!

[badge_issues]: https://img.shields.io/github/issues/svrooij/dotnet-feeder?style=flat-square
[badge_license]: https://img.shields.io/github/license/svrooij/dotnet-feeder?style=flat-square
[badge_sponsor]: https://img.shields.io/badge/Sponsor-at%20Github-red?style=flat-square

[link_issues]: https://github.com/svrooij/dotnet-feeder/issues
[link_license]: https://github.com/svrooij/dotnet-feeder/blob/main/LICENSE
[link_sponsor]: https://github.com/sponsors/svrooij
