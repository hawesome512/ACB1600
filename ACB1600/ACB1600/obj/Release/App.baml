   M S B A M L   `   `   `  ���� B  >ACB1600, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null!clr-namespace:ACB1600ACB1600      ACB1600.App   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     'localclr-namespace:ACB1600       �� 	Resources     ImageButtonTemplate �� 
TargetType   ACB1600.ImageButton ��Name j� Stretch j� Viewbox �� ImageSource �� RelativeSource �� Triggers P� Property	 P� Value
 �� 
TargetName �� Property �� Value �� 
TargetType �� Triggers �� Padding �� CornerRadius �� TextAlignment  ComboBoxToggleButton ��Name  ComboBoxTextBox �� 	ClickMode d� 	IsChecked �� Path �� Mode J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels  � ActualWidth! �� MaxDropDownHeight" K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation#   DirectionalNavigation$ P� 
SourceName��%�  &	        '�� �    '	� !    '��     &	 �    &	 5    '�� e    7  7  �� "  .R�� .+��� $ bgrect������ $ UniformToFill=�$ 
0,0,0.25,1� �� *	ImgPath+ �� *TemplatedParent+�� $��Center=�$��Center=�$��24(�
����������� *	Content+ �� *TemplatedParent+ P�  �@J�IsMouseOver	 . . ��� $
 bgrect�� � �� �� $ UniformToFill=�$ 0.25,0,0.25,1� �� *	ImgPath+ �� *TemplatedParent+P�  � ��	 . . ��� $
 bgrect�� � �� �� $ UniformToFill=�$ 0.5,0,0.25,1� �� *	ImgPath+ �� *TemplatedParent+P�  � {�	 .  . ��� $
 bgrect�� � �� �� $ UniformToFill=�$ 0.75,0,0.25,1� �� *	ImgPath+ �� *TemplatedParent+�� " �� P�  �@J�IsMouseOver	 . . ���  � �� �	#FF1ABC9C��  � ��$ 2,2,2,2q��� " 	� P�  �@d�	IsChecked$
	 TrueY�. ���  � �� �	#FFFF6501P�  �@d�	IsChecked$	 FalseY�. ���  � �� �	#FF5A5A5AP�  �@J�IsMouseOver	 . . ���  � ��
 �  ��.���  � ��$	 105����  � ��$ 35����  � ��$
 Left=���  � �� �� .R�� .+�F� +�������    �    ���hx��@,B    	���� *
Background+ �� *TemplatedParent+�� $��Center=�$��Center=�
����������� *	Content+ �� *TemplatedParent+�� " ��.���  �@J�SnapsToDevicePixels . ��  � ��$ Stretch=���  � ��$ Stretch=���  � ��$ 16(���  � ��$
 Bold"���  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� $��Border��$ 5q�$��2q�$��2q�$ 0��
������ 
������ ._��� $ Center=���  P�  �@��IsHighlighted	 . . ��� $
 Border�� � ��
 ������� $
 Border�� � ��
 � ����� " d�.���  � �� �� " d�.R�� .+��� $ Border��
�������#��z��$��2q� �� " ��.R��� $ PART_ContentHost��|�. �� " ��.���  � ��
 �   ���  � ��
 �������  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  � �� �� " ��.R�� .+�d� $��ToggleButton��#��z��#��z��#��z��8��  $��2��$ Press=� �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+�� $��ContentSite�� .  #��z #��z #��z��$��3q�$��Center=�$��Center=��� $ PART_EditableTextBox��8�� $��Center=�$��Center=�$��3q�|�. $y�Hidden=�# z ��Z� � $��Popup��$��Bottom=�#��z  . |�.  $��Slide=�.��� $��DropDown�� . #��z  #��z! .+��� $ DropDownBorder��
�������$��2q����	#FF1ABC9C�� $��4,6,4,6q� . .���� " . $# 	Contained=� P�  �@��HasItems	 .  . ��� $
 DropDownBorder�� � ��$ 95��P�  �@��
IsGrouping	 . . ���  � �� .  P� $$ Popup�� �@�AllowsTransparency	 . . ��� $
 DropDownBorder�� �@��CornerRadius$ 0���� $
 DropDownBorder�� � ��$ 0,2,0,0q�P�  �@��
IsEditable	 . . ���  � �� .  �� $
 PART_EditableTextBox�� � y�$ Visible=��� $
 ContentSite�� � y�$ Hidden=� P�  �@J�IsMouseOver	 . . ���  � �� �	#FF1ABC9C