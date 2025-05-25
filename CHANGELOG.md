# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.7.0] - 2025-05-25
### Added
- Added multitarget for .NET 9 and `Microsoft.AspNetCore.Components.Web` version `9.0.5` for that target.
### Fixed
- Fixed that Error Handling JSInterop did not work for Firefox.

## [0.6.0] - 2024-10-23
### Added
- Added `Float64Array`, `Int8Array`, `Int16Array`, `Int32Array`, `Float64ArrayInProcess`, `Int8ArrayInProcess`, `Int16ArrayInProcess`, and `Int32ArrayInProcess` types.
- Added target for .NET 8.
### Changed
- Changed version of `Microsoft.AspNetCore.Components.Web` from `7.0.3` to `7.0.20` when targeting .NET 7.

## [0.5.1] - 2024-06-23
### Fixed
- Fixed that the `SyntaxError` was erroneously mapped as a `TypeErrorException`.

## [0.5.0] - 2024-03-09
### Added
- Added `Float32ArrayInProcess`, `Uint8ArrayInProcess`, `Uint16ArrayInProcess`, `Uint32ArrayInProcess`, `ArrayBufferInProcess`, `SharedArrayBufferInProcess` as in-process counterpart to the asynchronous typed arrays and buffers.

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
