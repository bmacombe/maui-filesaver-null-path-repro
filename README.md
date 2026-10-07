# FileSaver Android null-path reproduction

A minimal .NET MAUI app that compares the two `CommunityToolkit.Maui` FileSaver overloads when running on Android.

The project pins `CommunityToolkit.Maui` 14.1.1, the version used when the Android `NullReferenceException` was reported.

## Run

Install the .NET 10 SDK and .NET MAUI Android workload, then build the Android target:

```powershell
dotnet build .\FileSaverNullPathRepro.csproj -f net10.0-android
```

To reproduce on-device:

1. Open `FileSaverNullPathRepro.csproj` in Visual Studio.
2. Select an Android device or emulator and run the app.
3. Tap **Save with null initialPath (repro)**.
4. Compare with **Save without initialPath (control)**.

## Expected behavior

On Android when `Android.OS.Environment.ExternalStorageDirectory` is available, the repro call passes `null` to the overload whose `initialPath` parameter is non-nullable:

```csharp
FileSaver.SaveAsync(null!, fileName, stream)
```

In CommunityToolkit.Maui 14.1.1, the Android implementation calls `initialPath.Replace(...)` without checking `initialPath`, so the result contains a `NullReferenceException` before the document picker opens. The control uses the dedicated overload without an initial path and should open the save picker.

The null argument is intentionally invalid for the path-taking overload. The repro is useful for determining whether the toolkit should improve its null validation and diagnostic, as well as confirming the Android-specific failure. It does not include any application-specific code or customer data.

Relevant source:

- [Android FileSaver implementation at 14.1.1](https://github.com/CommunityToolkit/Maui/blob/14.1.1/src/CommunityToolkit.Maui.Core/Essentials/FileSaver/FileSaverImplementation.android.cs)
- [Shared FileSaver overloads at 14.1.1](https://github.com/CommunityToolkit/Maui/blob/14.1.1/src/CommunityToolkit.Maui.Core/Essentials/FileSaver/FileSaverImplementation.shared.cs)
