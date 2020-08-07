module App.View

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.FontAwesome

type Model =
    { Value : string; IntValue : int; ColorSet : ResizeArray<string>}

type Msg =
    | Initialize
    | InitializeDelayed
    | ChangeColor of string
    | ChangeValue of string
    | ChangeIntValue of int

let init _ = { Value = ""; IntValue = 2; ColorSet =new ResizeArray<string>() }, Cmd.ofMsg Initialize
let d3ContainerId1 = "d3container1"
let d3ContainerId2 = "d3container2"
let private update msg model =
    match msg with
    | Initialize ->
        D3Showcase.AddD3ElementsToContainer d3ContainerId1
        // if we're too fast, D3 wouldn't find the elements to change, so we add a little delay (inspired by https://zaid-ajaj.github.io/the-elmish-book/#/chapters/commands/async-updates)
        // However, there will be probably a better solution, maybe wait for the 'event' when rendering is completed?
        let initializeDelayedCmd (dispatch: Msg -> unit) : unit =
          let delayedDispatch = async {
              do! Async.Sleep 1000
              dispatch InitializeDelayed
          }
          Async.StartImmediate delayedDispatch
        {model with Value = "initializing..."}, Cmd.ofSub initializeDelayedCmd
    | InitializeDelayed ->
        D3Showcase.ChangeSvgElementsWithD3
        {model with Value = ""}, Cmd.none
    | ChangeValue newValue ->
        { model with Value = newValue }, Cmd.none
    | ChangeColor newValue ->
        D3Showcase.ChangeCircleColorsWithD3 newValue

        { model with Value = newValue }, Cmd.none
    | ChangeIntValue newValue ->
        { model with IntValue = newValue}, Cmd.none

open System.Collections.Generic
let private view model dispatch =
                          D3ComponentShowcase.D3Component d3ContainerId2 {Color= model.Value; Count=model.IntValue; ColorSet = model.ColorSet }
  (*
    Hero.hero [ Hero.IsFullHeight ]
        [ Hero.body [ ]
            [ Container.container [ ]
                [ Columns.columns [ Columns.CustomClass "has-text-centered" ]
                    [ Column.column [ Column.Width(Screen.All, Column.IsOneThird)
                                      Column.Offset(Screen.All, Column.IsOneThird) ]
                        [ Image.image [ Image.Is128x128
                                        Image.Props [ Style [ Margin "auto"] ] ]
                            [ img [ Src "assets/fulma_logo.svg" ] ]
                          Field.div [ ]
                            [ Label.label [ ]
                                [ str "Enter your name" ]
                              Control.div [ ]
                                [ Input.text [ Input.OnChange (fun ev -> dispatch (ChangeValue ev.Value))
                                               Input.Value model.Value
                                               Input.Props [ AutoFocus true ] ] ] ]
                          Content.content [ Content.Modifiers [] ]
                            [ str "Hello, "
                              str model.Value
                              str " "
                              Icon.icon [ ]
                                [ Fa.i [ Fa.Regular.Smile ]
                                    [ ] ] ]
                          Container.container [ ] [
                              svg [
                                Fable.React.Props.SVGAttr.Width "150px"
                                Fable.React.Props.SVGAttr.Height "30px"
                                // Fable.React.Props.SVGAttr.ViewBox "-0.15 -0.65 10.3 10.3"; unbox ("width", "40%")
                                ] [
                                D3Showcase.customCircle dispatch ChangeColor 10 "pink"
                                D3Showcase.customRect dispatch ChangeColor 30 "red"
                                D3Showcase.customRect dispatch ChangeColor 60 "blue"
                                D3Showcase.customRect dispatch ChangeColor 90 "green"

                              ] ]
                          Container.container [ Container.Props [Id d3ContainerId1 ] ] [ ]
                          Container.container [] [
                                Control.div [ ]
                                  [ FunctionComponentsShowcase.InteractiveView  {|model ={Title=model.Value} |}]
                                Control.div [ ]
                                  [ FunctionComponentsShowcase.view2 {|count =model.IntValue; update= FunctionComponentsShowcase.updateDisp dispatch ChangeIntValue |} ]
                                Control.div [ ]
                                  [ FunctionComponentsShowcase.myView  { Title = model.Value} ]
                          ]
                          D3ComponentShowcase.D3Component d3ContainerId2 {Color= model.Value; Count=model.IntValue; ColorSet = model.ColorSet }

              ] ] ] ] ]
              *)

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run