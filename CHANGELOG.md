# [2.1.0](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare/v2.0.2...v2.1.0) (2022-11-09)


### Features

* Added support to .NET 7.0 ([4966500](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/4966500d11c0652a7711d686438991c52c0e00a2))

## [2.0.2](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare/v2.0.1...v2.0.2) (2022-09-03)


### Bug Fixes

* Chnages protected level of the `PROBLEM_MEDIA_TYPE_JSON` ([953bf23](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/953bf2362ef675930c2f666f2d5572b49795028f))

## [2.0.1](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare/v2.0.0...v2.0.1) (2022-09-03)


### Bug Fixes

* Chnages protected level of the `PROBLEM_MEDIA_TYPE_JSON`; ([de6ad18](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/de6ad188283044f892499d58a1ee867251142cbe))
* Property handler in IProblemFactory ([fddd15f](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/fddd15f43d0e36e89a742a2cf3c4441b40a755e3))

# [2.0.0](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare/v1.0.1...v2.0.0) (2022-09-03)


### Code Refactoring

* Add new ProblemDetails DTO `ErrorProblemDetails` extended from `ProblemDetails`; ([21c2e22](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/21c2e221bef21e8fe6f4ef2bbf4e29d9b323401d))
* Improved property handler ([38b827f](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/38b827f4c21570d126daaa8c81fcfcc86620e4db))
* rename default property from `RequestBody`  to `Payload` ([d13a56d](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/d13a56d942675da2fd2abf9539d522973994bd49))


### Features

* Add `TimeoutException` as default mapped exception to 504 status code ([cf42e26](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/cf42e26d0cbbc96116e319a7c67126e252e952d9))
* Add human-readable description in error list ([089cee3](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/089cee31e7daaf2417628beaf75446f556149e67))
* Add new `ExceptionMapper<T>(Status, Property, Code, Description)` in options; ([e5a01e4](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/e5a01e4a1c65e9e2737e128c4b6f0bb1e79b0661))
* Add support to costumize the status code link and title ([4ffde18](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/4ffde1809d0e6a9565515f20b9d3ca7cf5048ae9))
* Add support to debug in runtime `Microsoft.SourceLink.GitHub` ([e61dc02](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/e61dc02a4393e0acc33a55cbf5cf7043976492cb))
* Create `IProblemFactory` ([bfeac44](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/bfeac44c1def3a797abae8462cc5d792abf6dc6b))
* Override default `ProblemDetailsFactory` ([0fb082d](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/0fb082d82f577f74f0b6777af35e19611cab69dd))


### Tests

* Add support a tests para multi frameworks ([f072c98](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/commit/f072c98a22169e1f8e886ca3a2b38e573c172e51))


### BREAKING CHANGES

* Removed dependecy from `PowerUtils.Text`
* Change the structure of error list in `ErrorProblemDetails`
* Change `ApiProblemDetailsFactory` from public to internal
* Rename default property from `RequestBody`  to `Payload`
* Change `ProblemDetailsResponse` to `ErrorProblemDetails`
* Rename custom `ProblemDetailsFactory` from `ProblemDetailsFactory` to `ApiProblemDetailsFactory`
* Remove dependency from `PowerUtils.Net.Primitives`
* Remove support for `.NET 3.1`

# [1.1.1](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare/v1.1.0...v1.1.1) (2022-05-30)


### Fixes

- Fixed validation payload too large;




# [1.1.0](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare/v1.0.1...v1.1.0) (2022-05-29)


### Enhancements

- Added support to error code 413 when the payload too large;




# [1.0.1](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare/v1.0.0...v1.0.1) (2022-05-28)


### Fixes

- Fix `System.IndexOutOfRangeException` when format the properties name to camel case;




# 1.0.0 (2022-03-15)

- Kickoff;
- Moved the ErrorHandler from [PowerUtils.AspNetCore.WebAPI](https://github.com/TechNobre/PowerUtils.AspNetCore.WebAPI) project to this one so it can be used individually;


### Breaking Changes

- Extension `.AddProblemDetails();` named to `.AddErrorHandler()`;
- Extension `.UseProblemDetails();` named to `.UseErrorHandler()`;


### New Features

- Added options to be able to define the behavior of the error handler;


### Enhancements

- Normalized logs;
