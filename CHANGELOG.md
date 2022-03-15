# Changelog




## [1.0.0] - 2022-03-15

- Kickoff;
- Moved the ErrorHandler from [PowerUtils.AspNetCore.WebAPI](https://github.com/TechNobre/PowerUtils.AspNetCore.WebAPI) project to this one so it can be used individually;


### Breaking Changes

- Extension `.AddProblemDetails();` named to `.AddErrorHandler()`;
- Extension `.UseProblemDetails();` named to `.UseErrorHandler()`;


### New Features

- Added options to be able to define the behavior of the error handler;


### Enhancements

- Normalized logs;