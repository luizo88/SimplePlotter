   M S B A M L   `   `   `  ���� L  HSimplePlotterView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SimplePlotterViewSimplePlotterView  "    SimplePlotterView.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:SimplePlotterView     R NOxyPlot.Wpf, Version=2.1.2.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0Y UOxyPlot.Wpf.Shared, Version=2.1.2.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0"oxyhttp://oxyplot.org/wpf     � Title$  SimplePlotter��$
��1050��$
��1200�� � WindowState$ 	Maximized=� � Icon$* $Resources/SimplePlotterIcon16x16.ico�� � 	Resources  �� Source�� $a [pack://application:,,,/SimplePlotterView;component/WindowsResources/ResourceDictionary.xaml?�%    .���  � RowDefinitions �� $��23 ��� $��* � � ColumnDefinitions �� $
��Auto ��� $
��400* �.+�o� -   $��mainMenu��$��0,0,0,0q�$��3��.@�m� -   $��menuFile������ *Vocabulary.File+ �� FallbackValue$
 File��.@�m� -   $��menuOpen������ *Vocabulary.Open+$
 Open�� m� Command �� *OpenFileCommand+	 m� Icon	 �� $��Resources/Open.ico��$��16��m� -   $��menuSave������ *Vocabulary.Save+$
 Save�� �� *SaveFileCommand+	 �� $��Resources/Save.ico��$��16��m� -   $��menuLanguage������ *Vocabulary.Language+$ Language��.@�m� -   $��menuInterface��{�. ���� *Vocabulary.Interface+$ 	Interface��	 �� $��Resources/Language.ico��$��16��.@�m� -   $��	menuIntPT��$��PT��
 m� CommandParameter$
 0�� �� * InterfaceLanguageChangeCommand+	 �� $��Resources/PT.ico��$��16��m� -   $��	menuIntES��$��ES��$
 1�� �� * InterfaceLanguageChangeCommand+	 �� $��Resources/ES.ico��$��16��m� -	   $��	menuIntEN��$��EN��$
 2�� �� * InterfaceLanguageChangeCommand+	 �� $��Resources/EN.ico��$��16��m� -
   $��menuOptions������ *Vocabulary.Options+$ Options��.@�m� -   $��menuRestoreInitialConfig������ *!Vocabulary.RestoreInitialConfig+$ RestoreInitialConfig�� �� *RestoreInitialConfig+	 �� $��Resources/Restore.ico��$��16��m� -   $��menuPlot������ *Vocabulary.Plot+$
 Plot��.@�m� -   $��menuCopyToClipboard������ * Vocabulary.CopyPlotToClipboard+$ CopyPlotToClipboard�� �� *CopyPlotToClipboard+	 �� $��Resources/Clipboard.ico��$��16��m� -   $��menuExportPlot������ *Vocabulary.ExportPlot+$ 
ExportPlot�� �� *
ExportPlot+	 �� $��Resources/PNG.ico��$��16���� -   $��
lblVersion��$��Right=�$��2��$��0,0,0,0q����� *	Version+$ 
v. x.X.X.X��9� -   $��expd0��$��5,5,0,35q�$��1��$��0�� 9� ExpandDirection$ Right=� 9� 
IsExpanded . .���� $
��Auto=�$��0,0,0,0q�$��1��.��� ���	#FFF7FAFD$
��Left=�$	��750�� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�9� -   $��ExpdDataSeries��$��10,10,10,10q�$��0��
������� . ��]� $	��745������ *ActualHeight+ �� RelativeSource ��  �� AncestorType" d� �� Mode$ FindAncestor=�����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ DarkGray�� � Offset$ 1Q�� $ 	LightGray��.+��� -   $��txtDataSeries��$
��Bold"�$��Center=����� *Vocabulary.DataSeries+$ 
DataSeries��.��� $	��500��.+��� $
��Left=�$��5,10,0,0q�$	��Top=�$	��150��$��23������ *AddDataSeries+���� *Vocabulary.AddDataSeries+$ AddDataSeries���� $��5,40,5,0q�$	��140��$	��Top=�$��1������ *AvailableDataSeries+ �� SelectedItem �� *SelectedDataSeries+ �� Mode$ TwoWay=�����  �� 
TargetType" ��.���  �� Property � �� �� Value$
 Auto����  � ��$ Stretch=� �� View �� .P��� -   Name������ *Vocabulary.Name+$
 Name�� �� CellTemplate �� .R��� -    �� TextAlignment$
 Left=�$��0q�{�. ���� *Name+$ TwoWay=���Z� �� -   
XScale������ *Vocabulary.XScale+$ XScale�� �� .R��� -   $ Center=�$��0q�{�. ���� *XScale+$ TwoWay=���Z� �� -   
YScale������ *Vocabulary.YScale+$ YScale�� �� .R��� -   $ Center=�$��0q�{�. ���� *YScale+$ TwoWay=���Z� �� -   	Thick������ *Vocabulary.Thick+$ Thick�� �� .R��� -   $ Center=�$��0q�{�. ���� *Thick+$ TwoWay=���Z� �� -   	Style������ *Vocabulary.Style+$ Style�� �� .R��� -   $��0q����� *AvailableLineStyles+ �� *	LineStyle+�� -   
Marker������ *Vocabulary.Marker+$ Marker�� �� .R��� -   $��0q����� *AvailableMarkerTypes+ �� *
MarkerType+�� -   
MarkerSize������ *Vocabulary.MarkerSize+$ 
MarkerSize�� �� .R��� -    $ Center=�$��0q�{�. ���� *
MarkerSize+$ TwoWay=���Z� �� -!   	Color������ *Vocabulary.Color+$ Color�� �� .R��� -"   $��0q����� *AvailableColors+ �� *Color+{��� *StandardColor+�� -#   CustomColor������ *Vocabulary.CustomColor+$ CustomColor�� �� .R��� -$   $�� ��$��Center=� d� 	IsChecked �� *CustomColor+��Z� �� -%   RGB������ *Vocabulary.RGB+$	 RGB�� �� .R��� -&   $ Center=�$��0q����� *RGBDescription+$ TwoWay=���Z� {��� *CustomColor+�� -'   
Legend������ *Vocabulary.Legend+$ Legend�� �� .R��� -(   $�� ��$��Center=� �� *Legend+��Z� �� -)   SecondY������ *Vocabulary.SecondY+$ SecondX�� �� .R��� -*   $�� ��$��Center=� �� *	SecondY+��Z� �� $
��Left=�$��	5,190,0,0q�$	��Top=�$	��150��$��23������ *RemoveDataSeries+���� *Vocabulary.RemoveDataSeries+$ RemoveDataSeries���� $
��Left=�$��165,190,0,0q�$	��Top=�$��50��$��23������ *DataSeriesUp+���� *Vocabulary.ArrowUp+$ ArrowUp���� $
��Left=�$��225,190,0,0q�$	��Top=�$��50��$��23������ *DataSeriesDown+���� *Vocabulary.ArrowDown+$ 	ArrowDown���  �� $	��420 ��� $	��308 �.+��� $��10,220,220,0q�$	��Top=�$��0������ *Vocabulary.NumberOfPoints+$ NumberOfPoints���� $��10,250,200,0q�$	��Top=�$��0������ *!Vocabulary.CompressingAlgorithm+$ 	Algorithm���� $��10,280,200,0q�$	��Top=�$��0������ * Vocabulary.AlgorithmParameter1+$ AlgorithmParameter1���� $��10,340,200,0q�$	��Top=�$��0������ *" Vocabulary.NumberOfDecimalPlaces+$ NumberOfDecimalPlaces���� $��220,220,20,0q�$	��Top=�$��0��$��23�� �� 
IsReadOnly . ���� *NumberOfPoints+ �� UpdateSourceTrigger$ 	LostFocus=��� $��220,250,20,0q�$	��Top=�$��0��$��23������ * AvailableCompressingAlgorithms+ �� *SelectedCompressingAlgorithms+�� $��220,280,20,0q�$	��Top=�$��0��$��23������ *AlgorithmParameter1+$ 	LostFocus=��� $��220,310,20,0q�$	��Top=�$��0��$��23������ *ReduceNumberOfPoints+���� *!Vocabulary.ReduceNumberOfPoints+$ ReduceNumberOfPoints���� $��220,340,130,0q�$	��Top=�$��0��$��23������ *NumberOfDecimalPlaces+$ 	LostFocus=��� $��300,340,20,0q�$	��Top=�$��0��$��23������ *RoundDataSeriesPoints+���� *Vocabulary.Round+$ Round���� $��	5,200,5,0q�$	��265��$	��Top=�$��1������ *SelectedDataSeriesPoints+���� " ��.���  � ��$
 Auto����  � ��$ Stretch=� �� .P��� -+   	OriginalX������ *Vocabulary.OriginalX+$ 	OriginalX�� �� .R��� -,   $ Center=�$��0q�{�. ���� *X+$ TwoWay=���Z� �� --   	OriginalY������ *Vocabulary.OriginalY+$ 	OriginalY�� �� .R��� -.   $ Center=�$��0q�{�. ���� *Y+$ TwoWay=���Z� �� -/   ScaledX������ *Vocabulary.ScaledX+$ ScaledX�� �� .R��� -0   $ Center=�$��0q� . ���� *	ScaledX+��Z� �� -1   ScaledY������ *Vocabulary.ScaledY+$ ScaledY�� �� .R��� -2   $ Center=�$��0q� . ���� *	ScaledY+��Z� �� $��10,470,500,0q�$	��Top=����� *Vocabulary.ColorTemplate+$ ColorTemplate���� $��200,470,0,0q�$	��Top=�$
��Left=�$��23��$	��150������ *AvailableColorTemplates+ �� *SelectedColorTemplate+�� $��370,470,0,0q�$	��Top=�$
��Left=�$��23��$	��100������ *ApplyColorTemplate+���� *Vocabulary.Apply+$ Apply��9� -3   $��ExpdAxisConfiguration��$��10,10,10,10q�$��1��
������� .  ��]� $	��495������ *ActualHeight+ �� " d�$ FindAncestor=����� $ 0.5,1&�$ 0.5,0&�.��� $ DarkGray��$ 1Q�� $ 	LightGray��.+��� -4   $��txtAxisConfiguration��$
��Bold"�$��Center=����� *Vocabulary.AxisConfiguration+$ AxisConfiguration��.��� $	��250�� �� $	��240 ��� $	��280 �.+��� $��
10,10,10,0q�$	��Top=�$��0������ *Vocabulary.XLimits+$ XLimits���� $��
10,40,10,0q�$	��Top=�$��0������ *Vocabulary.YLimits+$ YLimits���� $��
10,70,10,0q�$	��Top=�$��0������ *Vocabulary.Y2Limits+$ Y2Limits���� $��10,100,10,0q�$	��Top=�$��0������ *Vocabulary.XAxisTitle+$ 
XAxisTitle���� $��10,130,10,0q�$	��Top=�$��0������ *Vocabulary.YAxisTitle+$ 
YAxisTitle���� $��10,160,10,0q�$	��Top=�$��0������ *Vocabulary.Y2AxisTitle+$ Y2AxisTitle���� $��10,190,10,0q�$	��Top=�$��0������ *Vocabulary.LogarithmicScale+$ LogarithmicScale���� $��10,220,10,0q�$	��Top=�$��0������ *Vocabulary.AxisLabelsStyle+$ AxisLabelsStyle���� $��
3,13,262,0q�$	��Top=�$��1��$��15�� �� *ManualXMinAxisLimit+�� $��20,10,210,0q�$	��Top=�$��1��$��23������ *
XAxisMin+$ 	LostFocus=�{��� *ManualXMinAxisLimit+�� $��93,13,172,0q�$	��Top=�$��1��$��15�� �� *ManualXMaxAxisLimit+�� $��110,10,120,0q�$	��Top=�$��1��$��23������ *
XAxisMax+$ 	LostFocus=�{��� *ManualXMaxAxisLimit+�� $��
3,43,262,0q�$	��Top=�$��1��$��15�� �� *ManualYMinAxisLimit+�� $��20,40,210,0q�$	��Top=�$��1��$��23������ *
YAxisMin+$ 	LostFocus=�{��� *ManualYMinAxisLimit+�� $��93,43,172,0q�$	��Top=�$��1��$��15�� �� *ManualYMaxAxisLimit+�� $��110,40,120,0q�$	��Top=�$��1��$��23������ *
YAxisMax+$ 	LostFocus=�{��� *ManualYMaxAxisLimit+�� $��
3,73,262,0q�$	��Top=�$��1��$��15�� �� *ManualY2MinAxisLimit+{��� *HasSecondYAxis+�� $��20,70,210,0q�$	��Top=�$��1��$��23������ *	Y2AxisMin+$ 	LostFocus=�{��� *ManualY2MinAxisLimit+�� $��93,73,172,0q�$	��Top=�$��1��$��15�� �� *ManualY2MaxAxisLimit+{��� *HasSecondYAxis+�� $��110,70,120,0q�$	��Top=�$��1��$��23������ *	Y2AxisMax+$ 	LostFocus=�{��� *ManualY2MaxAxisLimit+�� $��20,100,20,0q�$	��Top=�$��1��$��23������ *
XAxisTitle+$ 	LostFocus=��� $��20,130,20,0q�$	��Top=�$��1��$��23������ *
YAxisTitle+$ 	LostFocus=��� $��20,160,20,0q�$	��Top=�$��1��$��23������ *Y2AxisTitle+$ 	LostFocus=�{��� *HasSecondYAxis+�� $��X��$��20,190,20,0q�$	��Top=�$��1��$��15�� �� *XLogarithmicScale+�� $��Y��$��100,190,20,0q�$	��Top=�$��1��$��15�� �� *YLogarithmicScale+�� $��Y2��$��180,190,20,0q�$	��Top=�$��1��$��15�� �� *Y2LogarithmicScale+{��� *HasSecondYAxis+�� $��20,220,180,0q�$	��Top=�$��1��$��23������ *AvailableAxisLabelFormats+ �� *SelectedXAxisLabelFormat+�� $��100,220,100,0q�$	��Top=�$��1��$��23������ *AvailableAxisLabelFormats+ �� *SelectedYAxisLabelFormat+�� $��180,220,20,0q�$	��Top=�$��1��$��23������ *AvailableAxisLabelFormats+ �� *SelectedY2AxisLabelFormat+{��� *HasSecondYAxis+9� -5   $��ExpdGridLines��$��10,10,10,10q�$��2��
������� .  ��]� $	��495������ *ActualHeight+ �� " d�$ FindAncestor=����� $ 0.5,1&�$ 0.5,0&�.��� $ DarkGray��$ 1Q�� $ 	LightGray��.+��� -6   $��txtGridLines��$
��Bold"�$��Center=����� *Vocabulary.GridLines+$ 	GridLines��.��� $	��130�� �� $	��240 ��� $	��280 �.+��� $��
10,10,10,0q�$	��Top=�$��0������ *Vocabulary.MajorGridLines+$ MajorGridLines���� $��
10,40,10,0q�$	��Top=�$��0������ *Vocabulary.MinorGridLines+$ MinorGridLines���� $��
10,70,10,0q�$	��Top=�$��0������ *Vocabulary.MajorStep+$ 	MajorStep���� $��10,100,10,0q�$	��Top=�$��0������ *Vocabulary.MinorStep+$ 	MinorStep���� $��X��$��
20,13,20,0q�$	��Top=�$��1��$��15�� �� *XMajorGridLines+�� $��Y��$��110,13,20,0q�$	��Top=�$��1��$��15�� �� *YMajorGridLines+�� $��Y2��$��200,13,20,0q�$	��Top=�$��1��$��15�� �� *Y2MajorGridLines+{��� *HasSecondYAxis+�� $��X��$��
20,43,20,0q�$	��Top=�$��1��$��15�� �� *XMinorGridLines+�� $��Y��$��110,43,20,0q�$	��Top=�$��1��$��15�� �� *YMinorGridLines+�� $��Y2��$��200,43,20,0q�$	��Top=�$��1��$��15�� �� *Y2MinorGridLines+{��� *HasSecondYAxis+�� $��20,70,200,0q�$	��Top=�$��1��$��23������ *
XMajorStep+$ 	LostFocus=��� $��110,70,110,0q�$	��Top=�$��1��$��23������ *
YMajorStep+$ 	LostFocus=��� $��200,70,20,0q�$	��Top=�$��1��$��23������ *Y2MajorStep+$ 	LostFocus=�{��� *HasSecondYAxis+�� $��20,100,200,0q�$	��Top=�$��1��$��23������ *
XMinorStep+$ 	LostFocus=��� $��110,100,110,0q�$	��Top=�$��1��$��23������ *
YMinorStep+$ 	LostFocus=��� $��200,100,20,0q�$	��Top=�$��1��$��23������ *Y2MinorStep+$ 	LostFocus=�{��� *HasSecondYAxis+9� -7   $��ExpdChartBox��$��10,10,10,10q�$��3��
������� .  ��]� $	��495������ *ActualHeight+ �� " d�$ FindAncestor=����� $ 0.5,1&�$ 0.5,0&�.��� $ DarkGray��$ 1Q�� $ 	LightGray��.+��� -8   $��txtChartBox��$
��Bold"�$��Center=����� *Vocabulary.ChartBox+$ ChartBox��.��� $	��160�� �� $	��240 ��� $	��280 �.+��� $��
10,10,10,0q�$	��Top=�$��0������ *Vocabulary.ChartSize+$ 	ChartSize���� $��
10,40,10,0q�$	��Top=�$��0������ *Vocabulary.ChartTitle+$ 
ChartTitle���� $��
10,70,10,0q�$	��Top=�$��0������ *Vocabulary.ShowLegend+$ 
ShowLegend���� $��10,100,10,0q�$	��Top=�$��0������ *Vocabulary.LegendPosition+$ LegendPosition���� $��10,130,10,0q�$	��Top=�$��0������ *Vocabulary.ShowLegendArrows+$ ShowLegendArrows���� $��20,10,200,0q�$	��Top=�$��1��$��23������ *
ChartWidth+$ 	LostFocus=��� $��110,10,110,0q�$	��Top=�$��1��$��23������ *ChartHeight+$ 	LostFocus=��� $��
20,40,20,0q�$	��Top=�$��1��$��23������ *
ChartTitle+$ 	LostFocus=��� $��
20,73,20,0q�$	��Top=�$��1��$��15�� �� *
ShowLegend+�� $��20,100,20,0q�$	��Top=�$��1��$��23������ *AvailableLegendPositions+ �� *SelectedLegendPosition+�� $��20,133,20,0q�$	��Top=�$��1��$��15�� �� *ShowLegendArrows+9� -9   $��ExpdChartFontStyle��$��10,10,10,10q�$��4��
������� .  ��]� $	��495������ *ActualHeight+ �� " d�$ FindAncestor=����� $ 0.5,1&�$ 0.5,0&�.��� $ DarkGray��$ 1Q�� $ 	LightGray��.+��� -:   $��txtFontStyle��$
��Bold"�$��Center=����� *Vocabulary.FontStyle+$ 	FontStyle��.��� $	��130�� �� $	��240 ��� $	��280 �.+��� $��
10,10,10,0q�$	��Top=�$��0������ *Vocabulary.FontName+$ FontName���� $��
10,40,10,0q�$	��Top=�$��0������ *Vocabulary.AxisFontSize+$ AxisFontSize���� $��
10,70,10,0q�$	��Top=�$��0������ *Vocabulary.TitleFontSize+$ TitleFontSize���� $��10,100,10,0q�$	��Top=�$��0������ *Vocabulary.LegendFontSize+$ LegendFontSize���� $��
20,10,20,0q�$	��Top=�$��1��$��23������ *AvailableFonts+ �� *SelectedFont+�� $��20,40,200,0q�$	��Top=�$��1��$��23������ *XAxisFontSize+$ 	LostFocus=��� $��110,40,110,0q�$	��Top=�$��1��$��23������ *YAxisFontSize+$ 	LostFocus=��� $��200,40,20,0q�$	��Top=�$��1��$��23������ *Y2AxisFontSize+$ 	LostFocus=�{��� *HasSecondYAxis+�� $��20,70,200,0q�$	��Top=�$��1��$��23������ *TitleFontSize+$ 	LostFocus=��� $��20,100,200,0q�$	��Top=�$��1��$��23������ *LegendFontSize+$ 	LostFocus=�9� -;   $��
ExpdColors��$��10,10,10,10q�$��5��
������� .  ��]� $	��495������ *ActualHeight+ �� " d�$ FindAncestor=����� $ 0.5,1&�$ 0.5,0&�.��� $ DarkGray��$ 1Q�� $ 	LightGray��.+��� -<   $��	txtColors��$
��Bold"�$��Center=����� *Vocabulary.Colors+$ Colors��.��� $	��100�� �� $	��240 ��� $	��280 �.+��� $��
10,10,10,0q�$	��Top=�$��0������ *Vocabulary.BackColor+$ 	BackColor���� $��20,10,150,0q�$	��Top=�$��1��$��23������ *AvailableColors+ �� *SelectedBackColor+{��� *StandardBackColor+�� $��160,13,20,0q�$	��Top=�$��1��$��15�� �� *CustomBackColor+�� $��180,10,20,0q�$	��Top=�$��1��$��23������ *BackColorRGBDescription+$ 	LostFocus=�{��� *CustomBackColor+�� $��
10,40,10,0q�$	��Top=�$��0������ *Vocabulary.BackgroundColor+$ BackgroundColor���� $��20,40,150,0q�$	��Top=�$��1��$��23������ *AvailableColors+ �� *SelectedBackgroundColor+{��� *StandardBackgroundColor+�� $��160,43,20,0q�$	��Top=�$��1��$��15�� �� *CustomBackgroundColor+�� $��180,40,20,0q�$	��Top=�$��1��$��23������ *BackgroundColorRGBDescription+$ 	LostFocus=�{��� *CustomBackgroundColor+�� $��
10,70,10,0q�$	��Top=�$��0������ *Vocabulary.GridLinesColor+$ GridLinesColor���� $��20,70,150,0q�$	��Top=�$��1��$��23������ *AvailableColors+ �� *SelectedGridLinesColor+{��� *StandardGridLinesColor+�� $��160,73,20,0q�$	��Top=�$��1��$��15�� �� *CustomGridLinesColor+�� $��180,70,20,0q�$	��Top=�$��1��$��23������ *GridLinesColorRGBDescription+$ 	LostFocus=�{��� *CustomGridLinesColor+9� -=   $��ExpdCreateGIF��$��10,10,10,10q�$��6��
������� .  ��]� $	��495������ *ActualHeight+ �� " d�$ FindAncestor=����� $ 0.5,1&�$ 0.5,0&�.��� $ DarkGray��$ 1Q�� $ 	LightGray��.+��� ->   $��txtCreateGIF��$
��Bold"�$��Center=����� *Vocabulary.CreateGIF+$ 	CreateGIF��.��� $	��160�� �� $	��220 ��� $	��300 �.+��� $��
10,10,10,0q�$	��Top=�$��0������ *Vocabulary.GIFType+$ GIFType���� $��
10,40,10,0q�$	��Top=�$��0������ *Vocabulary.TotalTime+$ 	TotalTime���� $��
10,70,10,0q�$	��Top=�$��0������ *Vocabulary.FramesPerSecond+$ FramesPerSecond���� $��10,100,10,0q�$	��Top=�$��0������ *Vocabulary.InterpolateData+$ InterpolateData���� $��20,10,120,0q�$	��Top=�$��1��$��23������ *AvailableGIFTypes+ �� *SelectedGIFType+{��� *StandardBackgroundColor+�� $��20,40,210,0q�$	��Top=�$��1��$��23������ *GIFTotalTime+$ 	LostFocus=��� $��20,70,210,0q�$	��Top=�$��1��$��23������ *GIFFramesPerSecond+$ 	LostFocus=�{��� *NeedGifFramesPerSecond+�� $��20,103,20,0q�$	��Top=�$��1��$��15�� �� *InterpolateData+{��� *NeedGifInterpolateData+�� $��20,130,210,0q�$	��Top=�$��1������ *Vocabulary.CreateGIF+$ 	CreateGIF������ *	CreateGIF+� $��1�� �� $
��Auto �.+��� $
��Left=�$��5,0,5,0q�$��Center=�$��0������ *Vocabulary.RefreshPlot+$ RefreshPlot������ *RefreshPlot+� -?   $��PlotRef1��$��1��$��1��$	��Top=�$
��Left=����� *
ChartWidth+���� *ChartHeight+� -@   $��PlotRef2��$��1��$��1��$��Stretch=�$��Stretch=�� -A   $��PlotGrid��$��1��$��1�� (   "SimplePlotterView.MaxSizeConverter   MaxSizeConverter��%   &	          .+�  OxyPlot.Wpf.PlotView  -B   $
��Plot��$��0,0,0,0q�  OxyPlot.Wpf.PlotViewBase   Model �� *	PlotObj+��`�   `� 	Converter#  [  .1��� ! �� ElementName$! PlotRef1��" �� Path$" Width��� $! PlotRef2��$" ActualWidth���`� #  [  .1��� $! PlotRef1��$" Height��� $! PlotRef2��$" ActualHeight�