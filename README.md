# What

> [!WARNING]
> This library is very much work in progress, expect issues.

This library tries to be a drop-in replacement for the `FSharp.Core` with functions, which are generally faster and more flexible than built-in ones, but may be backwards-incompatible at runtime/compile-time.

## Examples of current (and future planned) backwards-incompatibilities:
- Some collections functions (`min`, `max`, `sum`, etc) can handle things like `NaN` differently than `FSharp.Core`.
- All `try*` functions return `ValueOption<'T>`.
- `Option<'T>` is aliasing `ValueOption<'T>`, all `Option` module functions shadowing the ones from `FSharp.Core`, several helper functions/methods provided to convert back and from the `FSharp.Core.Option<'T>`.
- `...`

## Hot to use:
This library being a `drop-in` replacement doesn't mean that just referencing its NuGet package is enough. Shadowing is achieved by opening this library's namespace (works on any granularity), for example:

```fsharp
open FSharp.Core.Extended
```

will shadow every module and type defined in this library (e.g. `Option` type, `Option` module, `Array` module, `List` module, etc)


```fsharp
open FSharp.Core.Extended.Collections
```

will shadow every module and type defined for collections


```fsharp
open FSharp.Core.Extended.Collections.Array
```

will shadow every type and module defined for arrays


And so on.

## Latest [benchmarks](tests/benchmarks) from my Macbook Pro M2:
  
```
BenchmarkDotNet v0.13.10, macOS Sonoma 14.2 (23C5030f) [Darwin 23.2.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), Arm64 RyuJIT AdvSIMD DEBUG
  Job-DSPPPV : .NET 8.0.0 (8.0.23.47906), Arm64 RyuJIT AdvSIMD

Jit=RyuJit  Concurrent=True  Server=True

```
| Type                      | Method                           | ArraySize | Mean            | Error         | StdDev        | Completed Work Items | Lock Contentions | Gen0     | Exceptions | Allocated  |
|-------------------------- |--------------------------------- |---------- |----------------:|--------------:|--------------:|---------------------:|-----------------:|---------:|-----------:|-----------:|
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core&#39;        | 1000      |     30,256.4 ns |      99.25 ns |      87.98 ns |                    - |                - |   0.3662 |          - |    47952 B |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 1000      |        117.6 ns |       1.80 ns |       1.68 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core&#39;        | 10000     |    300,712.0 ns |   1,065.59 ns |     944.62 ns |                    - |                - |   3.9063 |          - |   479952 B |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 10000     |      1,427.0 ns |      17.18 ns |      16.07 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core&#39;        | 100000    |  3,016,762.7 ns |   7,449.48 ns |   6,603.77 ns |                    - |                - |  39.0625 |          - |  4799955 B |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 100000    |     14,613.6 ns |      58.70 ns |      54.91 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core&#39;        | 1000000   | 31,019,018.2 ns |  91,488.90 ns |  81,102.52 ns |                    - |                - | 375.0000 |          - | 47999975 B |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 1000000   |    146,386.1 ns |   1,389.49 ns |   1,299.73 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core&#39;        | 2000000   | 60,629,290.8 ns | 436,017.09 ns | 364,094.02 ns |                    - |                - | 750.0000 |          - | 96000044 B |
| ArrayMaxBenchmark&lt;Int32&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 2000000   |    294,907.8 ns |   2,919.77 ns |   2,731.15 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core&#39;        | 1000      |     30,196.9 ns |      63.21 ns |      59.13 ns |                    - |                - |   0.3662 |          - |    47952 B |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 1000      |        503.5 ns |       1.33 ns |       1.24 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core&#39;        | 10000     |    301,220.3 ns |     753.45 ns |     667.92 ns |                    - |                - |   3.9063 |          - |   479952 B |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 10000     |      5,702.3 ns |       9.89 ns |       8.26 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core&#39;        | 100000    |  3,017,843.7 ns |  30,831.74 ns |  27,331.53 ns |                    - |                - |  39.0625 |          - |  4799955 B |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 100000    |     57,945.9 ns |     660.97 ns |     618.27 ns |                    - |                - |        - |          - |          - |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core&#39;        | 1000000   | 30,349,593.9 ns |  84,386.58 ns |  78,935.26 ns |                    - |                - | 333.3333 |          - | 48000001 B |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 1000000   |    586,054.1 ns |   3,875.16 ns |   3,624.83 ns |                    - |                - |        - |          - |        1 B |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core&#39;        | 2000000   | 74,387,851.6 ns | 772,736.35 ns | 685,010.65 ns |                    - |                - | 714.2857 |          - | 96000057 B |
| ArrayMaxBenchmark&lt;Int64&gt;  | &#39;Array.max - FSharp.Core.Extended&#39; | 2000000   |  1,189,814.5 ns |  16,878.39 ns |  15,788.06 ns |                    - |                - |        - |          - |        1 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core&#39;        | 1000      |     33,369.3 ns |      76.55 ns |      71.60 ns |                    - |                - |   0.3662 |          - |    47952 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core.Extended&#39; | 1000      |      2,023.4 ns |      10.25 ns |       9.59 ns |                    - |                - |        - |          - |       32 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core&#39;        | 10000     |    333,861.9 ns |   1,267.03 ns |   1,185.18 ns |                    - |                - |   3.9063 |          - |   479952 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core.Extended&#39; | 10000     |     20,025.4 ns |      64.02 ns |      59.89 ns |                    - |                - |        - |          - |       32 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core&#39;        | 100000    |  3,328,309.9 ns |   6,227.70 ns |   5,520.69 ns |                    - |                - |  39.0625 |          - |  4799955 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core.Extended&#39; | 100000    |    204,124.4 ns |   2,799.66 ns |   2,618.80 ns |                    - |                - |        - |          - |       32 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core&#39;        | 1000000   | 33,365,733.5 ns | 129,606.86 ns | 114,893.10 ns |                    - |                - | 333.3333 |          - | 48000001 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core.Extended&#39; | 1000000   |  2,012,827.2 ns |  13,210.22 ns |  12,356.84 ns |                    - |                - |        - |          - |       35 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core&#39;        | 2000000   | 67,436,583.3 ns | 785,460.42 ns | 696,290.20 ns |                    - |                - | 750.0000 |          - | 96000044 B |
| ArrayMaxBenchmark&lt;Double&gt; | &#39;Array.max - FSharp.Core.Extended&#39; | 2000000   |  3,972,294.0 ns |  17,859.48 ns |  16,705.77 ns |                    - |                - |        - |          - |       38 B |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core&#39;        | 1000      |     30,199.9 ns |      75.38 ns |      66.82 ns |                    - |                - |   0.3662 |          - |    47952 B |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 1000      |        116.9 ns |       1.26 ns |       1.18 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core&#39;        | 10000     |    302,357.9 ns |     958.90 ns |     800.72 ns |                    - |                - |   3.9063 |          - |   479952 B |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 10000     |      1,413.9 ns |       2.63 ns |       2.33 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core&#39;        | 100000    |  3,015,049.3 ns |   6,340.93 ns |   5,931.31 ns |                    - |                - |  39.0625 |          - |  4799955 B |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 100000    |     14,429.8 ns |      28.30 ns |      26.47 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core&#39;        | 1000000   | 30,453,994.4 ns | 129,116.10 ns | 120,775.28 ns |                    - |                - | 357.1429 |          - | 48000005 B |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 1000000   |    144,359.4 ns |     227.00 ns |     212.34 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core&#39;        | 2000000   | 60,428,326.1 ns | 254,932.11 ns | 225,990.67 ns |                    - |                - | 714.2857 |          - | 96000057 B |
| ArrayMinBenchmark&lt;Int32&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 2000000   |    289,552.9 ns |   1,346.62 ns |   1,259.63 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core&#39;        | 1000      |     30,228.6 ns |      78.74 ns |      73.66 ns |                    - |                - |   0.3662 |          - |    47952 B |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 1000      |        508.6 ns |       8.17 ns |       7.64 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core&#39;        | 10000     |    302,161.4 ns |   1,943.39 ns |   1,817.85 ns |                    - |                - |   3.9063 |          - |   479952 B |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 10000     |      5,859.3 ns |      60.13 ns |      56.24 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core&#39;        | 100000    |  3,027,836.1 ns |   8,373.09 ns |   6,991.91 ns |                    - |                - |  39.0625 |          - |  4799955 B |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 100000    |     58,233.3 ns |     749.00 ns |     700.62 ns |                    - |                - |        - |          - |          - |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core&#39;        | 1000000   | 30,948,529.4 ns |  83,593.15 ns |  74,103.15 ns |                    - |                - | 333.3333 |          - | 48000001 B |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 1000000   |    588,418.3 ns |   4,100.06 ns |   3,835.20 ns |                    - |                - |        - |          - |        1 B |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core&#39;        | 2000000   | 60,807,061.8 ns | 359,024.75 ns | 299,801.93 ns |                    - |                - | 714.2857 |          - | 96000057 B |
| ArrayMinBenchmark&lt;Int64&gt;  | &#39;Array.min - FSharp.Core.Extended&#39; | 2000000   |  1,183,861.7 ns |   7,259.66 ns |   6,790.69 ns |                    - |                - |        - |          - |        1 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core&#39;        | 1000      |     33,803.2 ns |     105.65 ns |      98.83 ns |                    - |                - |   0.3662 |          - |    47952 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core.Extended&#39; | 1000      |      2,002.9 ns |       5.84 ns |       4.88 ns |                    - |                - |        - |          - |       32 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core&#39;        | 10000     |    337,769.6 ns |   1,467.24 ns |   1,372.46 ns |                    - |                - |   3.9063 |          - |   479952 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core.Extended&#39; | 10000     |     19,943.2 ns |      65.47 ns |      61.24 ns |                    - |                - |        - |          - |       32 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core&#39;        | 100000    |  3,337,424.5 ns |  11,022.19 ns |  10,310.17 ns |                    - |                - |  39.0625 |          - |  4799955 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core.Extended&#39; | 100000    |    199,199.8 ns |     708.99 ns |     663.19 ns |                    - |                - |        - |          - |       32 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core&#39;        | 1000000   | 33,250,394.5 ns |  86,292.51 ns |  76,496.06 ns |                    - |                - | 375.0000 |          - | 47999998 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core.Extended&#39; | 1000000   |  1,980,354.2 ns |   5,570.30 ns |   5,210.46 ns |                    - |                - |        - |          - |       35 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core&#39;        | 2000000   | 66,489,180.2 ns | 118,303.97 ns | 110,661.61 ns |                    - |                - | 750.0000 |          - | 96000044 B |
| ArrayMinBenchmark&lt;Double&gt; | &#39;Array.min - FSharp.Core.Extended&#39; | 2000000   |  4,029,762.6 ns |  12,493.23 ns |  11,686.17 ns |                    - |                - |        - |          - |       38 B |
</details>
