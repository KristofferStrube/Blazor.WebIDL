# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.4.0] - 2024-02-22
### Added
- Added `DisposesJSReference` property to `IJSwrapper` interface and made it extend `IAsyncDisposable`.
- Made `IJSRuntime.GetHelperAsync` extension method available as a public method.
### Fixed
- Fixed that the `SyntaxError` DomException was not handled explicitly to be distinguishable from the Native Error of the same name.

## [0.3.0] - 2024-01-04
### Added
- Added `ArrayBuffer` and `SharedArrayBuffer` types and their common interface `IArrayBuffer` that can be used in methods that return either.
- Added `IArrayBufferView` interface to annotate objects that can get a reference to the `IArrayBuffer` that they are a view into.
- Added `ITransferable` interface to annotate objects that can be transferred across agents like `ArrayBuffer`.
- Added `Uint16Array` and `Uint32Array` types that are derived from `TypedArray` for representing uhsort and uint types.
- Added creator methods for creating any concrete `TypedArray`, either empty, from an `IArrayBuffer`, from another `TypedArray`, or from an initial length.
- Added `GetBufferAsync` method to `TypedArray` for getting the underlying `IArrayBuffer` from any `TypedArray` so that it implements `IArrayBufferView`.
- Added generic `GetCreatableAsync<T>` method on `ValueReference` for getting a value directly as a type that implements `IJSCreatable<T>`.
- Added `IJSWrapperConverter` attribute that can be used to annotate an `IJSWrapper` class as being JSON serializable.
### Fixed
- Fixed that `ErrorHandlingJSInProcessRuntime` and `ErrorHandlingJSInProcessObjectReference` would invoke async functions, that needed to be awaited, synchronously.


## [0.2.0] - 2023-08-15
### Added
- Added the deprecated DOMExceptions `IndexSizeErrorException`, `InvalidAccessErrorException`, `TypeMismatchErrorException`, and `URLMismatchErrorException`.
