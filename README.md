# dotnet-feeder

A dotnet tool for parsing rss feeds and modifying a markdown file

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

### Sample posts from my blog

By running `dotnet-feeder feed https://svrooij.io/feed.json ./README.md --count 5` your can generate the section below.

<!-- start posts -->
- [Protect against certificate extraction - encryption](https://svrooij.io/2022/06/01/certificate-extraction-encryption/)
- [Protect against certificate extraction - Client credentials](https://svrooij.io/2022/05/27/certificate-extraction-client-credentials/)
- [Extract all Azure AD admin accounts](https://svrooij.io/2022/05/17/extract-azure-admins/)
- [Extract all users with powershell and what you should do about it](https://svrooij.io/2022/05/16/extract-all-users-with-powershell/)
- [Deploy to Azure Static Web App with only the name](https://svrooij.io/2022/05/05/deploy-static-web-app-without-token/)
<!-- end posts -->
