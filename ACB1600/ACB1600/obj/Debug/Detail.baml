   M S B A M L   `   `   `  ����B  >ACB1600, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null!clr-namespace:ACB1600ACB1600  5   	   m iSystem.Windows.Forms.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�mclr-namespace:System.Windows.Forms.DataVisualization.Charting;assembly=System.Windows.Forms.DataVisualization/System.Windows.Forms.DataVisualization.Charting 5   	       ACB1600.Detail   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   'localclr-namespace:ACB1600     5   	   }Chrmclr-namespace:System.Windows.Forms.DataVisualization.Charting;assembly=System.Windows.Forms.DataVisualization     5   	   $	��200��5   	   .��5      � .+�5   
   �� 
���W�.�$
��0,12q�6(   $	��1,2q�66   ���	#FFE9EEF16e   �� 5	   
   -   5        $��img_top��5	      $��14��6   $	��120��6+   $
��Left=�67   $	��Top=�6R   $��Images/top.png��6j   �� 5
   
   -   5        $��
img_bottom��5
      $��14��6"   $	��120��6.   $��Bottom=�6:   $��Images/bottom.png��6U   $
��Left=�6q      ACB1600.ImageButton  5   
   -   5           Name$  	btn_close��5         ACB1600.ImageButton   Click$ btn_close_Click��6�   $��27��6/   $��26��6:   $
��5,14q�6F   $	��Top=�6T   $��Right=�6l      ImgPath$ /Images/关闭.png��6�      ImageButtonTemplate#��[  6�   6�   7  1System.Windows.Forms.Integration.WindowsFormsHost  5   
   -   5        $��myHost��5      $��
0,14,40,14q�6)   $y�Hidden=�6=      Child. 5      ;  5System.Windows.Forms.DataVisualization.Charting.Chart  5      � 5   
   -   5        $��mySwitch��5      $��40q�6   $y�Hidden=�6+    � ColumnDefinitions 5      �� 5      $��1* �6#   �� 5      $��1* �6#   �� 5      $��1* �6#   �� 5      $��1* �6#   �� 5      $��1* �6#   �� 5      $��1* �6#   �� 5      $��1* �6#   �� 5      $��1* �6#   5      5      5      