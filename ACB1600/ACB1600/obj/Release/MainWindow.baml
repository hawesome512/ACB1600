   M S B A M L   `   `   `  ���� B  >ACB1600, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null!clr-namespace:ACB1600ACB1600      ACB1600.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     'localclr-namespace:ACB1600     -     � Title$  
MainWindow��$	��800��$
��1000�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=�$��16(� � 	Resources    ACB1600.String2Brush   String2Brush��%   &	          .���  � RowDefinitions �� $��35 ��� $��35 ��� $	��35* ��� $��35 �.+��� 
���W�.�$��1q�$��4��� -   $��grid_content��$��2��$��1,0,1,1q����	#FFEAEFF1 � ColumnDefinitions �� $��5* ��� $��3* ��� $��2* ��� $��5* ��� $��3* ��� $��2* ��� $��5* ��� $��3* ��� $��2* ��� $��5* ��� $��3* ��� $��2* ��� $��5* ��� $��3* ��� $��2* �.+��� $��3��$��3��
���W�.�$	��1,0q��� $��9��$��3��
���W�.�$	��1,0q�   ACB1600.Detail  -     Name$ detail��$��15��$	��Top=� K� ZIndex$	 100��$y�Hidden=�   ACB1600.ImageButton  -     Name$ 	btn_write��   ACB1600.ImageButton	   Click$	 btn_write_Click��$��28��$��28��
   ImgPath$
 /Images/write.png��$
��Left=�$	��Top=�$��15��  ImageButtonTemplate#��[ $ 99��$y�Hidden=�� $��0������  �� ImageSource$) #/ACB1600;component/Images/title.png��.+��� $��ACB1600通信测试工具��
�������$��35��$��Center=�$��Center=�$��18(��� -    ��Name$ Title�� �� Orientation$ 
Horizontal=�$��RightToLeft=�$	��7,4q�.+�  -   $ 	btn_close��$	 btn_close_Click��$��27��$��26��$	��Top=�$
 /Images/关闭.png��#��[   -   $ btn_size��$	 btn_size_Click��$��27��$��LeftToRight=�$��26��$	��Top=�$
 /Images/最大化.png��#���    -   $ btn_min��$	 btn_min_Click��$��27��$��26��$	��Top=�$
 /Images/最小化.png��#��[ � -	   $��grid_rbs��$��1,0,1,0q�$��1�����	#FFFF6501.+��� -
   $��
rect_right��$��35���� -   $��	rect_left��$��30��$��35��$
��Left=��� -   $��panel_setting��$��
0,0,-300,0q�$��35��$��Right=�$ 
Horizontal=�$��RightToLeft=�$	 100������ *Fill+ �� ElementName$ 
rect_right��.+��� -   $��cbox_com��$	��100���� $��串口��
��������� -   $��tbox_address��$��35��$��Center=�$��Center=�.��1�� $��地址��
��������� -   $��tbox_repeat��$��35��$��Center=�$��Center=�.��1�� $��重复��
��������� -   $��img_setting��$��Images/previous.png��$��35��� -   $��grid_bottom��$��3�����	#FF5A5A5A$��1q�.+��� -   $��lbl_msg��$��10,0,0,0q����� *	Content+ �� 	Converter# [  $ lbl_msg��