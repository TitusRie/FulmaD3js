module App.View

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.FontAwesome

type Model =
    { Value: string
      IntValue: int
      ColorSet: ResizeArray<string>
      DataSet: ResizeArray<int> }

type Msg =
    | Initialize
    | InitializeDelayed
    | ChangeColor of string
    | ChangeValue of string
    | ChangeIntValue of int

let random = System.Random()

let updateDataSetFromModel model =
    while model.DataSet.Count > model.IntValue do
        model.DataSet.RemoveAt(model.DataSet.Count - 1)
    while model.DataSet.Count < model.IntValue do
        model.DataSet.Add(random.Next(3, 25))
    model

let circleData =
    new ResizeArray<D3ComponentShowcase.DataItem>()

circleData.Add { ItemColor = "red"; ItemYPos = 5 }
circleData.Add { ItemColor = "green"; ItemYPos = 15 }
circleData.Add { ItemColor = "blue"; ItemYPos = 5 }

let init _ =
    { Value = ""
      IntValue = 2
      ColorSet = new ResizeArray<string>()
      DataSet = new ResizeArray<int>() },
    Cmd.ofMsg Initialize

let d3ContainerId = "d3container"

let private update msg model =
    match msg with
    | Initialize ->
        // https://zaid-ajaj.github.io/the-elmish-book/#/chapters/commands/async-updates
        let initializeDelayedCmd (dispatch: Msg -> unit): unit =
            let delayedDispatch =
                async {
                    do! Async.Sleep 2000
                    dispatch InitializeDelayed
                }

            Async.StartImmediate delayedDispatch

        updateDataSetFromModel { model with Value = "initializing..." }, Cmd.ofSub initializeDelayedCmd
    | InitializeDelayed -> { model with Value = "" }, Cmd.none
    | ChangeValue newValue -> { model with Value = newValue }, Cmd.none
    | ChangeColor newValue ->
        D3Showcase.ChangeCircleColorsWithD3 newValue
        circleData.Add({ ItemColor = newValue; ItemYPos = 0 })
        { model with Value = newValue }, Cmd.none
    | ChangeIntValue newValue -> updateDataSetFromModel { model with IntValue = newValue }, Cmd.none

open System.Collections.Generic

let private view model dispatch =
    Hero.hero [ Hero.IsFullHeight ] [
        Hero.body [] [
            Container.container [] [
                Columns.columns [ Columns.CustomClass "has-text-centered" ] [
                    Column.column [ Column.Width(Screen.All, Column.IsThreeFifths)
                                    Column.Offset(Screen.All, Column.IsOneFifth) ] [
                        Container.container [] [
                            svg [ Fable.React.Props.SVGAttr.Width "150px"
                                  Fable.React.Props.SVGAttr.Height "30px"
                                  // Fable.React.Props.SVGAttr.ViewBox "-0.15 -0.65 10.3 10.3"; unbox ("width", "40%")
                                 ] [
                                D3Showcase.customRect dispatch ChangeColor 20 "red"
                                D3Showcase.customRect dispatch ChangeColor 50 "blue"
                                D3Showcase.customRect dispatch ChangeColor 80 "green"
                            ]
                            br []
                            svg [ Fable.React.Props.SVGAttr.Width "150px"
                                  Fable.React.Props.SVGAttr.Height "50px"
                                  // Fable.React.Props.SVGAttr.ViewBox "-0.15 -0.65 10.3 10.3"; unbox ("width", "40%")
                                 ] [
                                D3Showcase.customCircle dispatch ChangeColor 62 "pink"
                            ]
                        ]
                        Container.container [] [
                            Control.div [] [
                                FunctionComponentsShowcase.titleView { Title = model.Value }
                            ]
                            Control.div [] [
                                FunctionComponentsShowcase.InteractiveView {| model = { Title = model.Value } |}
                            ]
                            Control.div [] [
                                FunctionComponentsShowcase.UpDownButton
                                    {| count = model.IntValue
                                       update = FunctionComponentsShowcase.updateDisp dispatch ChangeIntValue
                                       delta = -1 |}
                                ofInt model.IntValue
                                str "Columns"
                                FunctionComponentsShowcase.UpDownButton
                                    {| count = model.IntValue
                                       update = FunctionComponentsShowcase.updateDisp dispatch ChangeIntValue
                                       delta = 1 |}
                            ]
                        ]
                        D3ComponentShowcase.D3Component d3ContainerId { ColorSet = circleData }
                        D3ComponentShowcase.D3Barchart "d3barchart" model.DataSet
                    ]
                ]
            ]
        ]
    ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
