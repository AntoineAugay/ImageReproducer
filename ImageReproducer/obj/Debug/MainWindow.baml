   M S B A M L   `   `   `  ����T  PEmgu.CV.UI, Version=3.2.0.2721, Culture=neutral, PublicKeyToken=7281126722ab4438;,clr-namespace:Emgu.CV.UI;assembly=Emgu.CV.UI
Emgu.CV.UI  5   	   J FImageReproducer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:ImageReproducer.ViewModelImageReproducer.ViewModel 5   	       ImageReproducer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =emui,clr-namespace:Emgu.CV.UI;assembly=Emgu.CV.UI     5   	   9local'clr-namespace:ImageReproducer.ViewModel     5   	     � Title$  
MainWindow��5	   	   $	��700��6   $
��1200��6)    � DataContext 5
      3 -ImageReproducer.ViewModel.MainWindowViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �  5      �� 5      $��* �6     5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $	��200 �6#   .+�5!      �� -   5         ��Name$ 	TargetImg��5!      $��0��6�   $��10q�6�   ��6'   �� *TargetImgPath+��6H   �� *
ImageWidth+��6e   �� *ImageHeight+6�   � 5#      $��1��6    5$      �� 5%      $��* �6$   �� 5&      $��50 �6$    5(      �� 5)      $��* �6'   .+�5,      7  1System.Windows.Forms.Integration.WindowsFormsHost  $��1��6#   $��0,50,0,0q�63   $��Center=�6�   $��Center=�6�   ��6E   �� *
ImageWidth+��6b   �� *ImageHeight+   Child. 5-         Emgu.CV.UI.ImageBox  -   5        BestImg��5-   $   5.      �� 50      -   5         ��Name$ 
Generation��50      $��1��6,    �� ContentStringFormat$ Génération n°{0:#}��6_   $��Center=�6�   $	��Top=�6�   ��69   �� *CurrentGeneration+6�   51      � 53      $��2��6   $��Center=�6#   $��Center=�6@    54      �� 55      $
��Auto �6$   �� 56      $
��Auto �6$   �� 57      $
��Auto �6$    59      �� 5:      $��* �6'   .+�5=      �� -   5         ��Name$ 
BtnOptions��5=      $��0��6-   $��Options��6:   $��5q�6b   $��30��6m   $	��150��6y   ��6�   �� *OptionsCommand+�� 5>      -   5        $ BtnLoad��5>      $��1��6-   $
��Load��6:   $��5q�6b   $��30��6m   $	��150��6y   ��6�   �� *LoadCommand+�� 5?      -   5        $ BtnStart��5?      $��2��6-   $��5q�6b   $��30��6m   $	��150��6y   ��6:   �� *ButtonStartStopName+��6�   �� *StartStopCommand+5A      5C      � 5E   
   $��1��6    5F      �� 5G      $��* �6     5I      �� 5J      $
��Auto �6#   �� 5K      $	��100 �6#   �� 5L      $
��Auto �6#   �� 5M      $��75 �6#   �� 5N      $
��Auto �6#   �� 5O      $	��150 �6#   .+�5R      �� -   5        $ LabelConstElapseTime��5R      $��0��62   $��5,5,0,5q�6B   $��Elapse Time (s) : ��6T   �� 5S      -   5        $ LabelElapseTime��5S      $��1��62   $��0,5,5,5q�6B   $ {0:0.00}��6x   $
��Left=�6�   ��6T   �� *TotalElapseTime+6�   �� 5U      -	   5        $" LabelConstMeanGenerationTime��5U      $��2��6:   $��5,5,0,5q�6J   $"��Mean generation time (ms) : ��6\   �� 5V      -
   5        $ LabelMeanGenerationTime��5V      $��3��6:   $��0,5,5,5q�6J   $ {0:0.00}��6�   $
��Left=�6�   ��6\   �� *GenerationMeanTime+6�   �� 5X      -   5        $ LabelConstBestFitness��5X      $��4��63   $��5,5,0,5q�6C   $��Best Fitness : ��6U   �� 5Y      -   5        $ LabelBestFitness��5Y      $��5��63   $��0,5,5,5q�6C   $
��Left=�6u   ��6U   �� *BestFitness+6u   5[      5]      5`      