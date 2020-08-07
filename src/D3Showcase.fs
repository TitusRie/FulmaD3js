
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
   member this.on(eventName: string, callback: obj->unit): obj =
        this?on(eventName, fun () -> callback jsThis)

// Usage:
// A. .on("mouseover", thisFunc (fill "pink"))
// B. .on("mouseover", fill "pink")

let fill style (this:obj) =
    do D3.d3.select(this).style("fill", style) |> ignore

let stroke style (this:obj) =
    do D3.d3.select(this).style("stroke", style)  |> ignore

let animateSecondStep (this:obj) =
    do D3.d3.select(this)
        .transition()
        .duration(4000.0)
        .attr("r", 40)
        |> ignore

let animateFirstStep (this:obj) =
    do D3.d3.select(this)
        .transition()
        .delay(0.0)
        .duration(1500.0)
        .attr("r", 10)
        .on("end", animateSecondStep)
        |> ignore

let ChangeSvgElementsWithD3 =
        D3.d3.selectAll("rect")
            // .style("fill", "orange")
            .style("stroke", "black")
            .style("stroke-width", "2")
            .on("mouseover", stroke "red")
            .on("mouseout", stroke "green")
            // .on("mouseover", fill "red")
            // .on("mouseout", fill "green")
            |> ignore
        D3.d3.selectAll("circle")
            .style("fill", "green")
            .attr("width", 100)
            .attr("height", 100)
            |> ignore

let AddD3ElementsToContainer containerId =
        let d3container : D3.Selection.Selection<obj,obj,Browser.Types.HTMLElement,obj option> =
              D3.d3.select(sprintf "#%s" containerId)
        let svg =
          d3container
            .append("svg")
            .attr("width", 50)
            .attr("height", 50)
        svg
          .append("circle")
          .style("stroke", "gray")
          .style("fill", "purple")
          .attr("r", 20)
          .attr("cx", 22)
          .attr("cy", 22)
          .on("mouseover", fill "pink")
          //.on("mouseover", thisFunc (fill "pink"))
          .on("mouseout", fill "purple")
          .on("mousedown", animateFirstStep)
            |> ignore

let ChangeCircleColorsWithD3 color =
        D3.d3.select("#d3container circle").style("fill", color) |> ignore
        D3.d3.select("#d3circle").style("fill", color) |> ignore

open Fable.React
open Fable.React.Props
let customCircle dispatch msg (xPos:int) (color:string) =
                                circle [
                                  Fable.React.Props.Id "d3circle"
                                  Fable.React.Props.SVGAttr.Cx (sprintf "%d" xPos)
                                  Fable.React.Props.SVGAttr.Cy "15"
                                  Fable.React.Props.SVGAttr.R "10"
                                  Fable.React.Props.SVGAttr.Fill color
                                  Fable.React.Props.OnClick (fun _ -> dispatch (msg color))  ] []
let customRect dispatch msg (xPos:int) (color:string) =
                                rect [
                                  Fable.React.Props.SVGAttr.X (sprintf "%d" xPos)
                                  Fable.React.Props.SVGAttr.Y "5"
                                  Fable.React.Props.SVGAttr.Width "20"
                                  Fable.React.Props.SVGAttr.Height "20"
                                  Fable.React.Props.SVGAttr.Rx "5"
                                  Fable.React.Props.SVGAttr.Ry "5"
                                  Fable.React.Props.SVGAttr.Fill color
                                  Fable.React.Props.OnClick (fun _ -> dispatch (msg color)) ] []
