module D3Showcase

// Integrating D3js into Fable-React (Fulma), based on https://github.com/fable-compiler/Fable/issues/1822

open Fable.Core
open Fable.Core.JsInterop // for accessing jsnative properties with '?'

// declare d3 for native use:
//let [<Global>] d3: obj = jsNative
// so we can access properties without type checking with '?'
// let fill style this =
//     do d3?select(this)?style("fill", style)

// But thanks to the D3 bindings in d3.fs we can used typed access:
// d3?select(this)?style("fill", style) => D3.d3.select(this).style("fill", style)

// NOTE: not all d3 bindings are already added to d3.fs.
// When using missing d3 functions, either use native access (with '?')
// Or add the necessary binding with ts2fable, see chapter How did I do from Showcase article above: https://github.com/fable-compiler/Fable/issues/1822

// solution A. from https://fable.io/repl/#?code=PYBwpgdgBAYghgIwDZgHQGFgCcwChSSyIobZoBSAzgJIQAuYWouuKdUA2gDwDiSwCOEgB8AXSgATAMwAuKAIBWUALxQFlAHJw6ASwBueVmHY6ISU2Ch0AFjsowArhADGUABQAzOW8UBaYU46dACUwSq4UJFQHk7uYf7RapQAKraULGzROkhIUJR0AJ4oVmnhUZLAklIA-JRgKM50bjZ2wbWFKG4ARB7ZSF0ANHkdYMEZxlBwEDoAttpgAMpgzsAQEgsMICV2ZVESldK19ctNLZRj5eXVdFhTlEE6q24Xl5HVEg63uk8AjAAMAJer2q2hu3SwgygABY-mMjOwprN5jAdFh8hswFszrtIvsqkcGqc0kDLtdbhB7t8IM8Iq8oO96nACm5YbTgR8vo9qf9AWzSaCsODIf8SVcnl1IBJITFoM8oAlEXMGEsVmsMVjieN2JQ4DMQCgFgA1HhyRQqKB8w51QndADEeh0AC8uqKoiCQOA1t1KHoAOYuvlXAXdADuOgkNmFvLp9ODXWsYB0vusdCjrNweJ1eoNxvdnok3WcqOcKADV3yRTA3puwAA1mBIV1fbcCmW3RXOj0+o2Q7YGG23nGIUMYSSQXQwV1nAAPSEAVlZQYngqnraGC7H4pmwAcdWABmH23sTlcnj6UC6Qh0zjAyAcDdCfOqW53e4cqaGZ0cLncvRyF97IIH03akum3XcwH2EMIEhL8T0maYlTAFE0TodUxiAA&html=DwQgIg8gwgKgmgBQKIAIAWAXAtgGwHwBQwmuhxApgIYAmhK9KwAzgMYBOAlgA4YpNssAvACJMGLkwBcAemnUAzACsmAOgD2bAOZz5KgG4BWFVg4A7FcuF5g01px5lpaKrSIAjNdQCedBsGoceigc1CJ6HABeVjYBer70wFx4UDgcLADWKFjkIDZJRNIe3o4k+EA&css=Q
// let inline thisFunc (f: (obj->unit)) =
//     fun () -> f jsThis

// solution B. from https://fable.io/repl/#?code=PYBwpgdgBAYghgIwDZgHQGFgCcwChSSyIobZoBSAzgJIQAuYWouuKdUA2gDwDiSwCOEgB8AXSgATAMwAuKAIBWUALxQFlAHJw6ASwBueXHQCe4KAGVjlBgFtUAeQQKwAY3YB3HXQAWuKP5swGwRGKB8dSlRgCAAKMAN6LUC5aywdCABzABooFyEkQRcAazlFAFphAFcILwBKUqcVP38W-3DKAH5ouIS6JLAcgDNqqBjaqArc-MKitUoAFW8I2pY2KEGdJCQoa2MUMKXKJtaJYEkpDsowFDcY9trLkxQYgCINrZec3ZQV1jB2OA1GzaMDmVzRCTmBggA4RY4tU7nS7XVx0O6HFatVodOhYQGULw6bqYrH+DoSSp43TdACMAAYGSTSR1tLjXlhPlAACx035rQE6YEMGA6LDWKFgGHteH+RHSZE3NH3ZrM3H4wnElVY8nXODGGK8rXYilUomxemMo0tFl0NkvDk5elMrFRWIvSASTkCoWg8EQSHQ52tAA+wigOgyEDIq3+OzgNhAKHMADUeA0lKotfKrorXgBiPQ6ABeLyD1rgIHA-telD0GVLVrJrKwr08Eh8nItZabtpbL28YAj3jonctuERlHjidBqZZlY9rxcopcKAb2O+YBruOARTAnJeGTxxjX1o3r3eSH37iWDBPPbtDu5huxzcXAA9OQBWZ-l3uL485N+zquq8NjAJUVzAAYj4XlALxCDoLhgMglR7sB3QvGBEFgOBI5DJs2wvNeXhoVqIGYeBVynO4EBekCIIimKdASiAzqhuGkbRkAA&html=DwQgIg8gwgKgmgBQKIAIAWAXAtgGwHwBQwmuhxApgIYAmhK9KwAzgMYBOAlgA4YpNssAvACJMGLkwBcAemnUAzACsmAOgD2bAOZz5KgG4BWFVg4A7FcuF5g01px5lpaKrSIAjNdQCedBsGoceigc1CJ6HABeVjYBer70wFx4UDgcLADWKFjkIDZJRNIe3o4k+EA&css=Q
type System.Object with
    member this.on(eventName: string, callback: obj -> unit): obj =
        this?on (eventName, (fun () -> callback jsThis))

// Usage:
// A. .on("mouseover", thisFunc (fill "pink"))
// B. .on("mouseover", fill "pink")

let fill style (this: obj) =
    do D3.d3.select(this).style("fill", style) |> ignore

let strokeWidth thickness (this: obj) =
    do D3.d3.select(this).style("stroke-width", thickness)
       |> ignore

let animateSecondStep (this: obj) =
    do D3.d3.select(this).transition().duration(4000.0).attr("r", 25)
       |> ignore

let animateFirstStep (this: obj) =
    do D3.d3.select(this).transition().delay(0.0).duration(1500.0).attr("r", 2).on("end", animateSecondStep)
       |> ignore

let ChangeCircleColorsWithD3 color =
    D3.d3.select("#d3circle").style("fill", color)
    |> ignore

let AddHoverEffectWithD3() =
        D3.d3.selectAll("rect, circle")
            .style("stroke", "lightgray")
            .on("mouseover", strokeWidth "2")
            .on("mouseout", strokeWidth "0")
            |> ignore
let ShowUIHintWithD3(rectsCirclesContainerId) = // NOTE: unit param () is necessary to get the function executed
        D3.d3.selectAll("circle")
            .transition().delay(0.0).duration(100.0).attr("r", 20)
            .transition().duration(200.0).attr("r", 25)
            |> ignore
        D3.d3.selectAll(sprintf "#%s rect" rectsCirclesContainerId)
            .transition().delay(0.0).duration(100.0).attr("width", 20).attr("height", 20)
            .transition().duration(200.0).attr("width", 25).attr("height", 25)
            |> ignore

open Fable.React

let customRect changeColor (xPos: int) (color: string) =
    rect [ Fable.React.Props.SVGAttr.X(sprintf "%d" xPos)
           Fable.React.Props.SVGAttr.Y "5"
           Fable.React.Props.SVGAttr.Width "25"
           Fable.React.Props.SVGAttr.Height "25"
           Fable.React.Props.SVGAttr.Rx "5"
           Fable.React.Props.SVGAttr.Ry "5"
           Fable.React.Props.SVGAttr.Fill color
           Fable.React.Props.OnClick(fun _ -> changeColor color) ] []

let customCircle changeColor (xPos: int) (color: string) =
    circle [ Fable.React.Props.Id "d3circle"
             Fable.React.Props.SVGAttr.Cx(sprintf "%d" xPos)
             Fable.React.Props.SVGAttr.Cy "25"
             Fable.React.Props.SVGAttr.R "20"
             Fable.React.Props.SVGAttr.Fill color
             Fable.React.Props.OnClick(fun _ -> changeColor color) ] []

type LocalModel = { XPos: int; Color: string; ChangeColor:string->unit }

let InteractiveCircle =
    FunctionComponent.Of(fun (props:LocalModel) ->
        // Keep a value ref during component's life cycle, initialized to None
        let selfRef = Hooks.useRef None // Hooks only work within function component

        circle [ Fable.React.Props.Id "d3circle"
                 Fable.React.Props.SVGAttr.Cx(sprintf "%d" (props.XPos))
                 Fable.React.Props.SVGAttr.Cy "25"
                 Fable.React.Props.SVGAttr.R "25"
                 Fable.React.Props.SVGAttr.Fill (props.Color)
                   // We can pass the ref object directly to the new RefHook prop
                   // to get a reference to the actual button element in the browser's doom
                 Fable.React.Props.RefValue selfRef
                 Fable.React.Props.OnClick(fun _ ->
                        props.ChangeColor props.Color
                        animateFirstStep( selfRef.current.Value))
        ]
                 [ ]
        )
