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