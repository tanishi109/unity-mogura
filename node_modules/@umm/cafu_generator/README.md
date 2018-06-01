# CAFU Generator

## What

* C# scripts generator for CAFU

## Requirement

* Unity 2018.1
* .NET 4.x / C# 6.0

## Install

```shell
yarn add "umm-projects/cafu_generator#^1.0.0"
```

## Usage

[![Youtube Video](https://img.youtube.com/vi/42t5AN8KcwM/0.jpg)](https://www.youtube.com/watch?v=42t5AN8KcwM)

* `Window` &gt; `CAFU` &gt; `Script Generator` <br />![image](https://user-images.githubusercontent.com/838945/40044057-3ce37f3a-5861-11e8-8dba-662322731b78.png)
* Select or Input options.<br />![image](https://user-images.githubusercontent.com/838945/40044135-7410cada-5861-11e8-9859-3fac06895064.png)
* Click `Generate` !!<br />![image](https://user-images.githubusercontent.com/838945/40044254-d3da9c84-5861-11e8-9027-9876c286d638.png)

### Options

| Name　　　　　　　　　　 | Description　　　　　　　　　　　　　　　 | Controller | Presenter | View | UseCase | Model | Translator | Repository | DataStore | Entity |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Structure Type | Structure (class) type to generate | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ |
| Scene Name | Scene name what generated class belongs | ✓ | ✓ | ✓ |  |  |  |  |  |  |
| Class Name | Name of class |  |  | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ |
| Is Singleton? | Enable if use `ISingleton` |  |  |  | ✓ | ✓ |  | ✓ | ✓ | ✓ |
| Use Presenter Factory? | Enable if need to generate Factory class for Presenter | ✓ |  |  |  |  |  |  |  |  |
| Use Factory? | Enable if need to generate Factory class |  | ✓ |  | ✓ | ✓ |  | ✓ | ✓ | ✓ |
| Overwrite? | Overwrite file if already exists | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ |

### Extensions

* WIP

## License

Copyright (c) 2018 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

