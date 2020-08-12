module FunctionComponentsShowcase
// https://fable.io/blog/Announcing-Fable-React-5.html#functions-vs-function-components
open Fable.React
open Fable.React.Props
open Browser.Types // for Event, Node, etc
open Fulma // for Container

let attachEvent (f: Event -> unit) (node: Node) (eventType: string) =
    node.addEventListener (eventType, f)
    { new System.IDisposable with
        member __.Dispose() = node.removeEventListener (eventType, f) }

type LocalModel = { Title: string }

let InteractiveView =
    FunctionComponent.Of<{| model: LocalModel |}>(fun props ->
        // Keep a value ref during component's life cycle, initialized to None
        let selfRef = Hooks.useRef None

        // useEffect
        // https://reactjs.org/docs/hooks-effect.html#example-using-hooks
        // side effects after rendering

        // Passing an empty array for dependencies tells React the effect should
        // only run when mounting (and the disposable when unmounting)
        Hooks.useEffectDisposable
            ((fun () ->
                (Browser.Dom.document, "mousedown")
                ||> attachEvent (fun ev ->
                        let menuEl: Element = selfRef.current.Value
                        if not (menuEl.contains (ev.target :?> _)) then
                            selfRef.current.Value.textContent <- "Clicked Outside!"
                            printfn "Clicked outside!")),
             [||])
        Container.container [] [
            h1 [] [ str props.model.Title ]
            button
                   // We can pass the ref object directly to the new RefHook prop
                   // to get a reference to the actual button element in the browser's doom
                   [ RefValue selfRef
                     OnClick(fun _ ->
                         printfn "Clicked inside!"
                         selfRef.current.Value.textContent <- "Clicked inside! :-)") ] [
                str "Click me"
            ]
        ])

let titleView (props: LocalModel) = div [] [ str props.Title ]

let UpDownButton (props: {| count: int
                            update: int -> unit
                            delta: int |}) =
    button [ OnClick(fun _ -> props.count + props.delta |> props.update)
             Disabled(props.count + props.delta <= 0) ] [
        str <| sprintf "%+d" props.delta
    ]

let updateDisp dispatch msg (intVal: int) = dispatch <| msg intVal
