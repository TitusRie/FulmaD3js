module D3ComponentShowcase
// https://fable.io/blog/Announcing-Fable-React-5.html#functions-vs-function-components
open Fable.React
open Fable.React.Props
open Fulma // for Container

type LocalModel = { Count:int; Color:string; ColorSet:ResizeArray<string>}

type DataItem = { ItemColor:string; ItemXPos: string}

open Fable.Core.JsInterop // for accessing jsnative properties with '?'

let D3Component containerId (props: LocalModel) =
        // div [] [str props.Color; ofInt props.Count]
        let d3container : D3.Selection.Selection<obj,obj,Browser.Types.HTMLElement,obj option> =
              D3.d3.select(sprintf "#%s" containerId)

        // NOTE: svg conainer defaults to 300x150px when calculation failes: https://www.w3.org/TR/CSS21/visudet.html#inline-replaced-width

        let svg =
          d3container
            .append("svg")
            .attr("width", 50)
            .attr("height", 50)
            // for i = 1 to props.Count do
            //     svg
            //       .append("circle")
            //       .style("stroke", "gray")
            //       .style("fill", props.Color)
            //       .attr("r", 20)
            //       .attr("cx", 22)
            //       .attr("cy", 22)

            //         |> ignore
            // svg
        // let svg2 = D3.d3.select(sprintf "#%s > svg" containerId)
        let circleData = new ResizeArray<DataItem>()
        circleData.Add {ItemColor="red"; ItemXPos="10"}
        circleData.Add {ItemColor="green"; ItemXPos="30"}
        circleData.Add {ItemColor="blue"; ItemXPos="50"}
        // let allCircles =
        //   svg
        //               .selectAll("circle")
        //               .data(circleData) // needs to be ResizeArray
        //               |> fun d3Selection  -> d3Selection.enter() // enter selection
        //               |> fun d3SelectionElement  -> // https://fable.io/docs/communicate/js-from-fable.html#Erase-attribute
        //                     d3SelectionElement.append("circle") // iterate through elements
        //                                         .attr("id", id)

        //                                         // .attr("fill", fun x -> x.) // access to data element
        //                                         //.attr("fill", id.Color) // id = fun x -> x
        //                                         .attr("fill", sprintf "%O" (unbox (id)).ItemColor) // id = fun x -> x
        //                                         .attr("r", 20)
        //                                         .attr("cy", 22)
        let random = System.Random()
        let dataset =  Array.init 3 (fun _ -> (random.Next(3,25)))
        let dataset2 = new ResizeArray<int>(dataset)
        let barHeight x = x * 5
        let barPadding = 1.
        let width = 500.
        let height = 100.
        let dataSetLength = float dataset.Length

        // let svg = D3.Globals.select("#display")
        //                     .append("svg")
        //                     .attr("width", U3.Case1 width)
        //                     .attr("height", U3.Case1 height)

        // svg.selectAll("rect")
        //     .data(dataset2)
        //     |> fun x -> x.enter()
        //     |> fun x -> x.append("rect")
        //     // |> fun x -> x
        //                   .attr("width", fun _ _ _ -> System.Math.Abs(width / dataSetLength - barPadding))
        //                   .attr("height", fun data _ _ -> float data * 4.)
        //                   .attr("x", fun _ x _ -> x * (width/dataSetLength))
        //                   .attr("y", fun data _ _ ->height - float data * 4.)
        //                   .attr("fill", fun data _ _ ->sprintf "rgb(63,%A,150)" (data * 10))
        // |> ignore


        svg.selectAll("text")
            .data(dataset2)
            .enter()
            .append("text")
        // |> fun (selection:D3.Selection.Selection<obj,int,obj,obj> ) -> selection.enter() // Selection<obj,int,obj,obj>
        // |> fun (element : D3.Selection.Selection<D3.Selection.EnterElement,int,obj,obj>) ->
        //       element.append("text")
        // |> fun x -> x
                     .text(fun x i -> sprintf "%O -- %O" x i)
                    //  .attr("x", fun x -> x * (width/dataSetLength))
                     .attr("y", fun data -> height - (float data * 4.))
        |> ignore
        Container.container [ Container.Props [Id containerId ]] []

// Above code in REPL: https://fable.io/repl/#?code=PTAECMEMGcFMBNQHsB2oBKBRACgGVAGYBOSAtqABYAuVADtAFwgDmAllRQK7gB0AxmWAFI4ADawAtANK1W4osABiI8cFbRonWNGABGABwAmQwCgTSWrDTKxsHgGEkRWOcvWVdx854ApaAEkUKlgSWjNxKlAAbQAeAHFRJChRAD4AXVB4AGYGZHAAK1AAXlB86AA5SCpWADcXEyoAT0tQAGVG6GDSHgB5Atg+SIB3dgoTUAnSWFJwENAOdR5UAApYOqDKqdzOolYUZgAaUD5IUVEoPgBrXKT8iRTOFHYAShuC4vGJr4mF6AB+FZrKxUTawI4ER6gZbPUD3Y6nc6QK6laAAFQo6me4VgkQIclEoE6jXE8wx0A+33gSEyWT+cHEg2Wv2edKa4mWACI8WcOUcieIsSYIqBIE9SFVYK0Bqh4K1grRSeoKV8qTS6bAGVQmWSsd9vn8qERRdB2KwVrq9RM-vBOEbqitdAAGZ0Wy1-KqGzlEXmgAAsjsFwtFrHFwUUrCInTlsAVv2VE1V2XVmu1mM+bsNxtN5vTeutGsgjWWAdz+ptdrNKGWTpdpa+7poRC9Pqdrr1SyrHKs8B9wdDkulKFl8rb3wAPilQKxmCgnPVhdBIDJxK0AGpxN6FEq5pP0gZajkAYhqrAAXhzR-XILQ3PBOdAaswL3WrR6mxyRvAOD6AKy1y2vo2nIULA07UD6-olkKOKEkutDiAAIlUkDFKAuYoLAQwYNoZ6wAAgkQRqNDEADeY7+F0jiJEQDA7HszAANygBR0wABrYEgjB7FQY4AL4pNCJiqouy6wEhVCQDweHwIgZEsaQVFOEUHLOD2THyexnFFE6fFCdSInwWJyFSTJoByZRSDUcpzDOFYHLqV0mnQEUWSOrpwlwYhxnSbJ5EWVZHJiFo9nMY5HHOX+ul6bBolrnEubJvueFnJyfARnw4jPmWyHLAZXkSTCICZMhoAYQg5JUNSszYSap74YRhYJcCITQqARXNUQhIavulYJdet6pelmWFWA7AhBKpIkJwzAUKA3VTEE0AJfysCctyog+hCaAAB6wpO21-PJilEG2DaeipEElvqb6pY0v5XV8RVne+fDbZtkK7awe2gH+oAAFRQqwADUujPDCbVgHGIxnKAzBWONwSgJghFOLkyMkDRoAxME21UJOb6sOAnCI40aPbZYgwIKA4j7BwRwco0ylbYMlaAwAJGzhgwiRoDOFQtpoL9APLKwHOGKAQOgKDDEcjwfVARyr3vTt30HRp4Vth2nKkEgnBwEgdTeuC+KgByTiinDWVfFrHI63rsC61Qm0m8sW2gLtcJq-5Thg7mNt23AVJDCgvZihK4aRlQ0a0G2E5TjOc5mB5sXrst3WDMloicjjTunfAOV5UZBUQ8VEmlbA5XzFVsA1bhBFEU1QQtSNc1N51e4s6gfU3t22ewLjF4l2Ndo1xwU0zXN4gLVQS36jnrsfVO31C4DINgyXcACEOoC0JARpTMEnUizCSr5xJY3kMPVROFOQ797mIDPZyd3govX1wivItry3UMm3DGEjyRijTG6NUZYxzvjRshNiY11Jkjcm+4qY02YHTU2jMuSPE7mgEWYtua8xxALH6jp-rs05hLKWzwZZy2ugrF+hBF73F0IYYhksvZsQ1rmOO05ZzOAAnw-h3wTBAA&html=DwQgIg8gwgKgmgBQKIAIAWAXAtgGwHwBQwmuhxApgIYAmhK9KwAzgMYBOAlgA4YpNssAvACJMGLkwBcAemnUAzACsmAOgD2bAOZz5KgG4BWFVg4A7FcuF5g01px5lpaKrSIAjNdQCedBsGoceigc1CJ6HABeVjYBeo4e3o4k+EA&css=Q

// Fable D3 Barchart sample remake in REPL
// https://fable.io/repl/#?code=FAehAICUFMGMCdoEMAuBLA9gO3BgZuAGJIBGANtOAEJLywAWtK4AykgLYAOF49KKnAM4AuMHlIUAdJhDjy0ALQBzeEk700sQSEEdu0bQBMAzCBK0GTEGiyHoAD0l92ZUBHODoh3DkgBRAAUAGXA8eAx2Xn4hURAlNBR6AFcSSVgI2QlFdK40CngQYnlrQUEkgxAARgAOACZa4GAMTmgcIqkAYQxEJpa2rMkuxEkAKUEASSwUaHDORopmAG0AHgBxMgxzMgA+AF1wE2FcEgArcABecBPBADlUNAA3aEaUAE8W1lfBafZJAHlTnBmAB3BL0YDgSHsaDsEgzcCJNCCSTYAAU0CeUzu0KO33gNiUABpwLAkGQyOZYABrI6bE4KbZJLAJACUtNOFwhkO5kMRggA-GiMa0UNjoMS8EzwKiWeAGSSyRSkNSroIACoaQQs+bQZh4PJkcDfV48PmcnmGDAHYz8zwUWAoVF8lm2t4UVEAIn15I9xONFG1wAWRr0FBYADVVuyzpcuZCTLboPbHR6AMQPNAALw92p5PP5aj6hk9ggeShzcbzBf48E9oMMiV94AArAAGVu5vOQ6soWse+jQNBKPhNgAs7cDwdUtgiF3AWGgwM+3xhkkgSBn7BlQd1B1QSE8zEuC6XMEEWegAEF4KpXssbChttLr7fpMzmLVm9LJTgAPpyp9UWnS1fhuBxHWMQlPxZGDtWDcx4AACUHYdmHsOd0IAKhbcAd2YBCAiQQxDAJOdKkkPDwHrRI5zbVsKODAchz4Mj2wY3dDH3FhdSCVolBoy48A2VA9xQA9dUGDAmRQRpLRDLgw0jStE2TS9yU9RAHQrfNOLE1FdPElBZTAUSkHnaAvEEBErThKADAvF8kFeZSRRmGVwBM1z4CNJMgUwLBlMLVpiw9TSUBzDyIASGZUEoRJwiSYdwF86EpkEQKazrNAG3oJsWC+H5JAAWVQehJEvEhBFRaj6A80zuJQXisH42qFHAAiiJI5qYIy3tPSY1Cmx-UyAJG7DR0kTtu1QPqPXsIapX-NBRuW7DquyxIQAMhqmpamDcPzGa+1eBacAM0aBpYtqhIwETzvGyber7b0yFOkb5UETh8SmAhQqUEhUQANkggBSS9CUqNsWQ9aV7vASoO21AAfJ8hywbpni7LtgDk3QFOgCNVmUu0gTUshPWmexwqm8B+QM-T90PGn+S87dDs4IsKfA7TuX5SnHWG875QMmmTL58DUUF-dwGW+VVvh9twAAalM4yIHGaZVBQboqO6KkrJsBFNRJbB+aez15olRaZZW8A1pqrauJ4vjEn2kyNZi7XvMtAwsAAchBPXeBmaAjj8G9uiOZZ+afI60BIJJpnAeww-sFoHS8cAKGaxJiS9JkHX86U0AAEhLypZQAb3ARAUCSeAcFL8u7elGq6u252c-oFkAG5wAAXw9c2PROq2zul+VLuYNrJeE5g4YmnqUZlpR0cQIA&html=DwQgIg8gwgKgmgBQKIAIAWAXAtgGwHwBQwmuhxApgIYAmhK9KwAzgMYBOAlgA4YpNssAvACJMGLkwBcAemnUAzACsmAOgD2bAOZz5KgG4BWFVg4A7FcuF5g01px5lpaKrSIAjNdQCedBsGoceigc1CJ6HABeVjYBeo4e3o4k+EA&css=Q
