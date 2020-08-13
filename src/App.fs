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
let rectsCirclesContainerId = "rectsCirclesContainerId"

let private update msg model =
    match msg with
    | Initialize ->
        D3Showcase.AddHoverEffectWithD3()

        // https://zaid-ajaj.github.io/the-elmish-book/#/chapters/commands/async-updates
        let initializeDelayedCmd (dispatch: Msg -> unit): unit =
            let delayedDispatch =
                async {
                    do! Async.Sleep 1500
                    dispatch InitializeDelayed
                }

            Async.StartImmediate delayedDispatch

        updateDataSetFromModel { model with Value = "initializing..." }, Cmd.ofSub initializeDelayedCmd
    | InitializeDelayed ->
        // after delay all D3 elements will be in the DOM and can be changed
        D3Showcase.ShowUIHintWithD3(rectsCirclesContainerId)
        { model with Value = "" }, Cmd.none
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
                        let dispatchChangeColor = fun color -> dispatch <| ChangeColor color
                        Container.container [ Container.Props [ Id rectsCirclesContainerId ] ] [
                            svg [ Fable.React.Props.SVGAttr.Width "150px"
                                  Fable.React.Props.SVGAttr.Height "30px"
                                 ] [
                                D3Showcase.customRect dispatchChangeColor 20 "red"
                                D3Showcase.customRect dispatchChangeColor 50 "blue"
                                D3Showcase.customRect dispatchChangeColor 80 "green"
                            ]
                            br []
                            svg [ Fable.React.Props.SVGAttr.Width "150px"
                                  Fable.React.Props.SVGAttr.Height "50px"
                                 ] [
                                D3Showcase.customCircle dispatchChangeColor 35 "pink"
                                D3Showcase.InteractiveCircle ( {XPos=85; Color="orange";ChangeColor=dispatchChangeColor})
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
                        D3ComponentShowcase.D3Component d3ContainerId { ColorSet = circleData;ChangeColor=dispatchChangeColor }
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
