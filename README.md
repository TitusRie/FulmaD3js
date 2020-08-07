# FableReactD3 Showcase based on Fulma minimal template


### How to create a [Fulma](https://fulma.github.io/Fulma/#template) application from scratch:

```
dotnet new -u Fable.Template.Fulma.Minimal
dotnet new -i Fable.Template.Fulma.Minimal
dotnet new fulma-minimal -n MyApp
cd MyApp
.\fake.sh build -t Watch <- On Mac / Linux / Unix
.\fake.cmd build -t Watch <- On Windows
```
For more info see [Fulma](https://mangelmaxime.github.io/Fulma/).


> **Note** _In case of error `Unsupported log file format. Latest supported version is 8, the log file has version 9.`
use: `dotnet paket update Fake.DotNet.cli -g netcorebuild` ([see this fix](https://github.com/fable-compiler/fable-elmish-electron-material-ui-demo/pull/14))_

> **Note** In VS Code activate F# View, then click the green play button and select _create launch.json file_


## Installing [D3js](https://d3js.org/)
> Thanks to [Maxime Mangel](https://github.com/MangelMaxime) for his [Fable D3js demo](https://github.com/MangelMaxime/fable-d3js-demo)

1. Add d3 dependency
```yarn add d3```

2. Add d3 bindings
Generate bindings with [ts2fable](https://fable.io/ts2fable/), see [this](https://github.com/fable-compiler/Fable/issues/1822#issuecomment-486270438) for detailed instructions
or just copy [D3js.fs](https://raw.githubusercontent.com/MangelMaxime/fable-d3js-demo/master/src/D3.fs) to your project.


> ### Background info
> Making React components (the new way): https://fable.io/blog/Announcing-Fable-React-5.html#functions-vs-function-components
> Making React components (old way, as a reference): https://blog.vbfox.net/2018/02/06/fable-react-1-react-in-fable-land.html
> Explanation how to integrate D3js into Fable: https://github.com/fable-compiler/Fable/issues/1822
> Using D3js within React: https://medium.com/technical-credit/declarative-d3-examples-in-react-6e736e526182
> Using D3js within React: https://wattenberger.com/blog/react-and-d3
> Integrating D3js into SAFE-Stack: https://stackoverflow.com/questions/52013274/safe-stack-with-d3-js
> Old article about integrating NVD3js into Fable: http://www.hoonzis.com/charting-with-fable-and-nvd3/
> Old Fable D3js example: https://fable.io/fable-graphics/#d3-js
> Old Fable D3js worldmap sample: https://github.com/fable-compiler/samples-browser/blob/master/src/d3map/App.fs
> Fable links: https://github.com/kunjee17/awesome-fable#examples

> https://www.npmjs.com/package/fable-import-d3

> https://blog.vbfox.net/2018/02/06/fable-react-1-react-in-fable-land.html
> https://fable.io/fable-graphics/samples/d3/barchart/index.html



### Update packages
To update all packages to their latest versions, possibly accros major versions and breaking your code use:
```
yarn upgrade --latest
```
For more info and more gentle upgrades see: https://classic.yarnpkg.com/en/docs/cli/upgrade/#toc-yarn-upgrade-package-latest-l-caret-tilde-exact-pattern

### Build for production

*If you are using Windows replace `./fake.sh` by `fake.cmd`*

1. Run: `./fake.sh build`
2. All the files needed for deployment are under the `output` folder.
