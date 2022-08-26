# Contributing

We would love for you to contribute to this project and help make it even better than it is today!
As a contributor, here are the guidelines we would like you to follow. It does help everyone to accept your Pull Requests with maximum awesomeness:

- [Code of Conduct](#code-of-conduct)
- [General Seps](#general-seps)
- [Commits and Pull Requests](#commits-pr)
  - [Conventional Commits](#conventional-commits)
    - [Common types you can use](#commits-types)
- [Coding Standards](#coding-standards)
- [Tests](#tests)



## <a name="code-of-conduct"></a> Code of Conduct

Please read and follow our [Code of Conduct][coc].



## <a name="general-seps"></a> General Seps

1. Check if there is already an open issue for the subject;
2. Open an issue to discuss the new feature;
3. Fork this repository;
4. Create your feature branch: `git checkout -b feat/my-new-feature`;
5. Add files changed:  `git add --all`;
6. Commit your changes: `git commit -m "feat: Add some feature"`;
7. Push to the branch: `git push origin feat/my-new-feature`;
8. Submit a pull request;


## <a name="commits-pr"></a> Commits and Pull Requests

* :x: **AVOID** breaking the continuous integration build.
* :heavy_check_mark: **DO** atomic commits to make it easier to analyze changes.
* :heavy_check_mark: **DO** keep pull requests small so they can be easily reviewed.
* :heavy_check_mark: **DO** only commit with conventional commit patterns

### <a name="conventional-commits"></a> Conventional Commits
To know more about conventional commits, visit [Conventional Commits](https://conventionalcommits.org/).

In general the pattern mostly looks like this:
```
<type>(<scope>): <short summary>
  │       │             │
  │       │             └─⫸ Summary in present tense. Not capitalized. No period at the end.
  │       │
  │       └─⫸ Commit Scope: *
  │
  └─⫸ Commit Type: feat|fix|docs|style|refactor|perf|build|test|ci|chore|merge|revert
```

Real world examples can look like this:
```
chore: run tests on travis ci
```
```
fix(server): send cors headers
```
```
feat(blog): add comment section
```

#### <a name="commits-types"></a> Common types you can use (based on the Angular convention):

- `feat:` A new feature
- `fix:` A bug fix
- `docs:` Documentation only changes
- `style:` Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
- `refactor:` A code change that neither fixes a bug nor adds a feature
- `perf:` A code change that improves performance
- `build:` Changes that affect the build system or external dependencies (example scopes: nugets, npm, SDKs, etc)
- `test:` Adding missing tests or correcting existing tests
- `ci:` Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs)
- `chore:` Other changes that don't modify src or test files
- `merge:` Merge a branch into other branch
- `revert:` Reverts a previous commit



## <a name="coding-standards"></a> Coding Standards
* :heavy_check_mark: **DO** add XML comment documentation to new classes, methods or parameters.
* :heavy_check_mark: **DO** add a test class for each feature and a test method for each 
* :heavy_check_mark: **DO** use language conventions to make code easy to understand quickly. See some tips here: [dofactory](https://www.dofactory.com/csharp-coding-standards);
* :heavy_check_mark: **CONSIDER** using SOLID patterns;



## <a name="tests"></a> Tests
* :heavy_check_mark: **DO** add a unit test if your Pull Requests resolves an issue or adds features.
* :heavy_check_mark: **CONSIDER** using test patterns like "AAA" and "Given When Then";
* :heavy_check_mark: **DO** add a test class for each feature and a test method for each assertion;
* :heavy_check_mark: **DO** make sure unit tests pass.
* :x: **AVOID** adding tests just to get coverage on sonarcloud.

***Disclaimer***
- 1️⃣ Unit in Unit Test is not a method/function.
- 2️⃣ One assertion per test doesn't mean invoking the Assert method only once.
> by @gsferreira
