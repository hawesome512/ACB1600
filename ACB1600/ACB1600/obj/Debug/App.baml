   M S B A M L   `   `   `  ����B  >ACB1600, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null!clr-namespace:ACB1600ACB1600  5          ACB1600.App   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      'localclr-namespace:ACB1600     5        �� 	Resources  5         ImageButtonTemplate �� 
TargetType   ACB1600.ImageButton ��Name j� Stretch j� Viewbox �� ImageSource �� RelativeSource �� Triggers P� Property	 P� Value
 �� 
TargetName �� Property �� Value �� 
TargetType �� Triggers �� Padding �� CornerRadius �� TextAlignment  ComboBoxToggleButton ��Name  ComboBoxTextBox �� 	ClickMode d� 	IsChecked �� Path �� Mode J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels  � ActualWidth! �� MaxDropDownHeight" K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation#   DirectionalNavigation$ P� 
SourceName��%�  &	        '�� 5    '	� �    '��     &	 �
    &	 �    '�� 2    7  7  �� 5   
   "  66   .R�5      � .+�5	      �� $ bgrect��6   ��5
      �� 5      $ UniformToFill=�6v   $ 
0,0,0.25,1�6�    6%   �� *	ImgPath+ �� *TemplatedParent+6�   5      �� 5      $��Center=�6e   $��Center=�6�   $��24(�6�   
���������6   �� *	Content+ �� *TemplatedParent+6�   5       5      P� 5       �@J�IsMouseOver	 . . �5      �� $
 bgrect��6    � �� 5      �� 5      $ UniformToFill=�6z   $ 0.25,0,0.25,1�6�    6)   �� *	ImgPath+ �� *TemplatedParent+6�   5      5      P� 5       � ��	 . . �5      �� $
 bgrect��6    � �� 5      �� 5      $ UniformToFill=�6z   $ 0.5,0,0.25,1�6�    6)   �� *	ImgPath+ �� *TemplatedParent+6�   5      5      P� 5       � {�	 .  . �5       �� $
 bgrect��6    � �� 5!      �� 5"      $ UniformToFill=�6z   $ 0.75,0,0.25,1�6�    6)   �� *	ImgPath+ �� *TemplatedParent+6�   5$      5%      5'      �� 5(   
   " ��6    5)      P� 5*       �@J�IsMouseOver	 . . �5+      ��  � �� �	#FF1ABC9C64   �� 5,       � ��$ 2,2,2,2q�68   5-      5/      �� 50   
   " 	�6    51      P� 52       �@d�	IsChecked$
	 TrueY�6/   . �53      ��  � �� �	#FFFF650163   54      P� 55       �@d�	IsChecked$	 FalseY�6/   . �56      ��  � �� �	#FF5A5A5A63   57      P� 58       �@J�IsMouseOver	 . . �59      ��  � ��
 �  ��63   5:      .�5<      ��  � ��$	 105��6&   �� 5=       � ��$ 35��6'   �� 5>       � ��$
 Left=�64   �� 5?       � �� 5@      �� 5A      .R�5B      � .+�5C      F� +�������    �    ���hx��@,B    	��6B   �� *
Background+ �� *TemplatedParent+�� 5D      $��Center=�6p   $��Center=�6�   
���������6$   �� *	Content+ �� *TemplatedParent+6�   5E      5F      5H      5I      �� 5K   
   " ��6   .�5L      ��  �@J�SnapsToDevicePixels . 64   �� 5M       � ��$ Stretch=�64   �� 5N       � ��$ Stretch=�62   �� 5O       � ��$ 16(�6)   �� 5P       � ��$
 Bold"�6+   �� 5Q       �@�OverridesDefaultStyle . 66   �� 5R       � �� 5S      �� 5T      " ��6&   .R�5U      �� $��Border��5V   #   $ 5q�5W   %   $��2q�5X   %   $��2q�5Y   %   $ 0��5Z   %   
������ 
������ ._�5^      �� $ Center=�6(   �� 5_   *   5`      5a       5b      P� 5c       �@��IsHighlighted	 . . �5d   "   �� $
 Border��6)    � ��
 �����6T   �� 5e   "   $
 Border��6)    � ��
 � ���6S   5f      5h      5j      5k      �� 5l   
   " d�6-   .�5m      ��  � �� 5n      �� 5o      " d�6&   .R�5p      � .+�5q      �� $ Border��6%   
�������#��z��6H   $��2q�6t   5r       5s      5u      5w      5x      �� 5y   
   " ��62   .R�5z      �� $ PART_ContentHost��6   |�. 6/   5{      �� 5|   
   " ��6   .�5}      ��  � ��
 �   �6+   �� 5~       � ��
 �����6+   �� 5       �@J�SnapsToDevicePixels . 64   �� 5�       �@�OverridesDefaultStyle . 66   �� 5�       � ��$
 Auto=�6K   �� 5�       � ��$
 Auto=�6I   �� 5�       � �� . 6>   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      � .+�5�      d� $��ToggleButton��5�   !   #��z��5�   !   #��z��5�   !   #��z��5�   !   8��  $��2��5�   !   $ Press=�5�   !    5�   !   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5�      �� 5�      $��ContentSite��5�   !    .  #��z 5�   !   #��z 5�   !   #��z��5�   !   $��3q�5�   !   $��Center=�5�   !   $��Center=�5�   !   �� 5�      $ PART_EditableTextBox��6&   8�� $��Center=�5�   !   $��Center=�5�   !   $��3q�5�   !   |�. $y�Hidden=�5�   !   # z 5�   !   ��5�   !   Z� 5�   !   � 5�      $��Popup��5�   !   $��Bottom=�5�   !   #��z 5�   !    . |�.  $��Slide=�5�   !   .��5�   "   � $��DropDown��5�   #    . #��z  5�   #   #��z! 5�   #   .+�5�   &   �� $ DropDownBorder��5�   )   
�������$��2q�5�   )   ���	#FF1ABC9C5�   )   �� 5�   &   $��4,6,4,6q�63    . .��5�   *   �� " . $# 	Contained=�6H   5�   '   5�   #   5�      5�       5�      P� 5�       �@��HasItems	 .  . �5�   "   �� $
 DropDownBorder��6)    � ��$ 95��6Z   5�      P� 5�       �@��
IsGrouping	 . . �5�   "   ��  � �� .  6R   5�      P� 5�      $$ Popup��6&    �@�AllowsTransparency	 . . �5�   "   �� $
 DropDownBorder��6)    �@��CornerRadius$ 0��6]   �� 5�   "   $
 DropDownBorder��6)    � ��$ 0,2,0,0q�6W   5�      P� 5�       �@��
IsEditable	 . . �5�   "   ��  � �� .  6>   �� 5�   "   $
 PART_EditableTextBox��6)    � y�$ Visible=�6d   �� 5�   "   $
 ContentSite��6)    � y�$ Hidden=�6X   5�      5�      5�       5�      P� 5�       �@J�IsMouseOver	 . . �5�      ��  � �� �	#FF1ABC9C65   5�      5�      5�      5�      