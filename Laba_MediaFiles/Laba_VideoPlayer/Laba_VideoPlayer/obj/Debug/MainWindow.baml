   M S B A M L   `   `   `  ����K  GLaba_VideoPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Laba_VideoPlayerLaba_VideoPlayer  5   	   !    Laba_VideoPlayer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Laba_VideoPlayer     5   	     � Title$  
MainWindow��5   	   $	��470��6   $	��646��6)   ��5	      �� 5
   
    �� ImageSource$ 
/wallp.png��6    j� TileMode$
 Tile=�6.    j� Stretch$ UniformToFill=�6>   .��5      � -   5         �Name$
 stop��5      .+�5   
   �� -   5         ��Name$ progress_bar��5      $
��Left=�6'   $��16��6B   $��
27,353,0,0q�6N   $	��Top=�6b   $	��596��6z   
���   �$��2��6�   �� 5   
   -   5         ��Name$	 dur��5      $�� ��6   $
��Left=�6(   $��28��6C   $��566,325,0,0q�6O   $	��Top=�6d   $��80��6|   
�������6�   �� 5   
   -   5        $ 
now_moment��5      $�� ��6$   $
��Left=�6/   $��28��6J   $��
10,325,0,0q�6V   $	��Top=�6j   $��80��6�   
�������6�   �� 5   
   -   5        $ volume��5      $
��Left=�6!   $��16��6<   $��489,399,0,0q�6H   $	��Top=�6]   $	��134��6u    J� RenderTransformOrigin$ 0.5,0.5&�6�   
���   � � LargeChange$
 0.01Q�6�   	 � Maximum$	 1Q�6�   
 � Value$	
 0.2Q�6�   
���   �6�   r� 5   
   -   5         r�Name$ vplayer��5      $
��Left=�6(   $	��297��6C   $��	27,21,0,0q�6P   $	��Top=�6c   $	��587��6{    r� UnloadedBehavior$ Manual=�6�    r� Volume$	 0.2Q�6�    r� ScrubbingEnabled . $ 0.5,0.5&�6�   4  .System.Windows.Automation.AutomationProperties   IsOffscreenBehavior$ Onscreen=�6�    r� LoadedBehavior$
 Play=�6  �� 5   
   -   5         ��Name$
 play��5      $�� ��6   $
��Left=�6*   $��245,374,0,0q�6E   $	��Top=�6Z   $��50��6r   $��50��6~   
�������
���   � J� OpacityMask 5      �� 5      $ 	/play.png��6   5      �� 5   
   -   5        $
 load��5      $�� ��6   $
��Left=�6*   $��
27,374,0,0q�6E   $	��Top=�6Y   $��50��6q   $��50��6}   
���   �
������� 5      �� 5      $ /download.png��6   5      �� 5   
   -	   5        $ pause��5      $�� ��6    $
��Left=�6+   $��300,374,0,0q�6F   $	��Top=�6[   $��50��6s   $��50��6   
���   �
������� 5      �� 5      $ 
/pause.png��6   5       �� 5!   
   -
   5        $��S��5!      $
��Left=�6   $��50��68   $��355,374,0,0q�6D   $	��Top=�6Y   $��50��6q   
���   � 5"      �� 5#      ��5%      �� 5&      5(      5)      5*      