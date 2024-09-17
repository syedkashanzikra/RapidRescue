$(document).ready(function($){var $wrapper=$('.main-wrapper');var $pageWrapper=$('.page-wrapper');var $slimScrolls=$('.slimscroll');var $sidebarOverlay=$('.sidebar-overlay');var Sidemenu=function(){this.$menuItem=$('#sidebar-menu a');};function init(){var $this=Sidemenu;$('#sidebar-menu a').on('click',function(e){if($(this).parent().hasClass('submenu')){e.preventDefault();}
if(!$(this).hasClass('subdrop')){$('ul',$(this).parents('ul:first')).slideUp(350);$('a',$(this).parents('ul:first')).removeClass('subdrop');$(this).next('ul').slideDown(350);$(this).addClass('subdrop');}else if($(this).hasClass('subdrop')){$(this).removeClass('subdrop');$(this).next('ul').slideUp(350);}});$('#sidebar-menu ul li.submenu a.active').parents('li:last').children('a:first').addClass('active').trigger('click');}
init();function sidebar_overlay($target){if($target.length){$target.toggleClass('opened');$sidebarOverlay.toggleClass('opened');$('html').toggleClass('menu-opened');$sidebarOverlay.attr('data-reff','#'+$target[0].id);}}
$(document).on('click','#mobile_btn',function(){var $target=$($(this).attr('href'));sidebar_overlay($target);$wrapper.toggleClass('slide-nav');$('#chat_sidebar').removeClass('opened');return false;});$(document).on('click','#task_chat',function(){var $target=$($(this).attr('href'));console.log($target);sidebar_overlay($target);return false;});$sidebarOverlay.on('click',function(){var $target=$($(this).attr('data-reff'));if($target.length){$target.removeClass('opened');$('html').removeClass('menu-opened');$(this).removeClass('opened');$wrapper.removeClass('slide-nav');}
return false;});if($('.toggle-password').length>0){$(document).on('click','.toggle-password',function(){$(this).toggleClass("feather-eye-off feather-eye");var input=$(".pass-input");if(input.attr("type")=="password"){input.attr("type","text");}else{input.attr("type","password");}});}
if($('.confirm-password').length>0){$(document).on('click','.confirm-password',function(){$(this).toggleClass("feather-eye-off feather-eye");var input=$(".pass-input-confirm");if(input.attr("type")=="password"){input.attr("type","text");}else{input.attr("type","password");}});}
function animateElements(){$('.circle-bar2').each(function(){var elementPos=$(this).offset().top;var topOfWindow=$(window).scrollTop();var percent=$(this).find('.circle-graph2').attr('data-percent');var animate=$(this).data('animate');if(elementPos<topOfWindow+$(window).height()-30&&!animate){$(this).data('animate',true);$(this).find('.circle-graph2').circleProgress({value:percent/100,size:400,thickness:30,fill:{color:'#2E37A4'}});}});}
if($('.circle-bar').length>0){animateElements();}
$(window).scroll(animateElements);if($('.select').length>0){$('.select').select2({minimumResultsForSearch:-1,width:'100%'});}
if($('.floating').length>0){$('.floating').on('focus blur',function(e){$(this).parents('.form-focus').toggleClass('focused',(e.type==='focus'||this.value.length>0));}).trigger('blur');}
if($('#msg_list').length>0){$('#msg_list').slimscroll({height:'100%',color:'#878787',disableFadeOut:true,borderRadius:0,size:'4px',alwaysVisible:false,touchScrollStep:100});var msgHeight=$(window).height()-124;$('#msg_list').height(msgHeight);$('.msg-sidebar .slimScrollDiv').height(msgHeight);$(window).resize(function(){var msgrHeight=$(window).height()-124;$('#msg_list').height(msgrHeight);$('.msg-sidebar .slimScrollDiv').height(msgrHeight);});}
if($slimScrolls.length>0){$slimScrolls.slimScroll({height:'auto',width:'100%',position:'right',size:'7px',color:'#ccc',wheelStep:10,touchScrollStep:100});var wHeight=$(window).height()-60;$slimScrolls.height(wHeight);$('.sidebar .slimScrollDiv').height(wHeight);$(window).resize(function(){var rHeight=$(window).height()-60;$slimScrolls.height(rHeight);$('.sidebar .slimScrollDiv').height(rHeight);});}
var pHeight=$(window).height();$pageWrapper.css('min-height',pHeight);$(window).resize(function(){var prHeight=$(window).height();$pageWrapper.css('min-height',prHeight);});if($('.datetimepicker').length>0){$('.datetimepicker').datetimepicker({format:'DD/MM/YYYY',icons:{up:"fas fa-angle-up",down:"fas fa-angle-down",next:'fas fa-angle-right',previous:'fas fa-angle-left'}});}
if($('.summernote').length>0){$('.summernote').summernote({placeholder:'Description',focus:true,minHeight:100,disableResizeEditor:false,toolbar:[['fullscreen',],['fontname',['fontname']],['undo'],['redo'],['datetimepicker'],['fontsize',['fontsize']],['font',['bold','italic','underline','clear']],['color',['color']],['para',['ul','ol','paragraph']],['insert',['link','picture']]],});}
if($('#summernote').length>0){$('#summernote').summernote({height:300,minHeight:null,maxHeight:null,focus:true});}
if($('#editor').length>0){ClassicEditor.create(document.querySelector('#editor'),{toolbar:{items:['heading','|','fontfamily','fontsize','|','alignment','|','fontColor','fontBackgroundColor','|','bold','italic','strikethrough','underline','subscript','superscript','|','link','|','outdent','indent','|','bulletedList','numberedList','todoList','|','code','codeBlock','|','insertTable','|','uploadImage','blockQuote','|','undo','redo'],shouldNotGroupWhenFull:true}}).then(editor=>{window.editor=editor;}).catch(err=>{console.error(err.stack);});}
if($('.counter').length>0){$('.counter').counterUp({delay:20,time:2000});}
if($('#timer-countdown').length>0){$('#timer-countdown').countdown({from:180,to:0,movingUnit:1000,timerEnd:undefined,outputPattern:'$day Day $hour : $minute : $second',autostart:true});}
if($('#timer-countup').length>0){$('#timer-countup').countdown({from:0,to:180});}
if($('#timer-countinbetween').length>0){$('#timer-countinbetween').countdown({from:30,to:20});}
if($('#timer-countercallback').length>0){$('#timer-countercallback').countdown({from:10,to:0,timerEnd:function(){this.css({'text-decoration':'line-through'}).animate({'opacity':.5},500);}});}
if($('#timer-outputpattern').length>0){$('#timer-outputpattern').countdown({outputPattern:'$day Days $hour Hour $minute Min $second Sec..',from:60*60*24*3});}
if($('.clipboard').length>0){var clipboard=new Clipboard('.btn');}
if($('.popover-list').length>0){var popoverTriggerList=[].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
var popoverList=popoverTriggerList.map(function(popoverTriggerEl){return new bootstrap.Popover(popoverTriggerEl)})}
$(".next").on('click',function(){$(this).closest('.tab-pane').next().css("display","block").css("opacity","1");$(this).closest('.tab-pane').css({'display':'none'});});$(".previous").on('click',function(){$(this).closest('.tab-pane').prev().css("display","block");$(this).closest('.tab-pane').css({'display':'none'});});if($('[data-bs-toggle="tooltip"]').length>0){$('[data-bs-toggle="tooltip"]').tooltip();}
if($('.custom-file-container').length>0){var firstUpload=new FileUploadWithPreview('myFirstImage')
var secondUpload=new FileUploadWithPreview('mySecondImage')}
if($('#editor').length>0){ClassicEditor.create(document.querySelector('#editor'),{toolbar:{items:['heading','|','fontfamily','fontsize','|','alignment','|','fontColor','fontBackgroundColor','|','bold','italic','strikethrough','underline','subscript','superscript','|','link','|','outdent','indent','|','bulletedList','numberedList','todoList','|','code','codeBlock','|','insertTable','|','uploadImage','blockQuote','|','undo','redo'],shouldNotGroupWhenFull:true}}).then(editor=>{window.editor=editor;}).catch(err=>{console.error(err.stack);});}
if($('.datatable').length>0){$('.datatable').DataTable({"bFilter":false,});}
if($('#datetimepicker3').length>0){$(function(){$('#datetimepicker3').datetimepicker({format:'LT',icons:{up:"fas fa-angle-up",down:"fas fa-angle-down",next:'fas fa-angle-right',previous:'fas fa-angle-left'}});});}
if($('#datetimepicker4').length>0){$(function(){$('#datetimepicker4').datetimepicker({format:'LT',icons:{up:"fas fa-angle-up",down:"fas fa-angle-down",next:'fas fa-angle-right',previous:'fas fa-angle-left'}});});}
if($('.center').length>0){$('.center').slick({centerMode:true,arrows:false,centerPadding:'30px',slidesToShow:3,responsive:[{breakpoint:768,settings:{arrows:false,centerMode:true,centerPadding:'40px',slidesToShow:3}},{breakpoint:480,settings:{arrows:false,centerMode:true,centerPadding:'40px',slidesToShow:3}}]});}
if($('[data-toggle="tooltip"]').length>0){$('[data-toggle="tooltip"]').tooltip();}
$(document).on('click','#open_msg_box',function(){$wrapper.toggleClass('open-msg-box');return false;});if($('#lightgallery').length>0){$('#lightgallery').lightGallery({thumbnail:true,selector:'a'});}
if($('#incoming_call').length>0){$('#incoming_call').modal('show');}
if($('.dash-count .counter-up').length>0){$('.dash-count .counter-up').counterUp({delay:15,time:1500});}
if($('.summernote').length>0){$('.summernote').summernote({height:200,minHeight:null,maxHeight:null,focus:false});}
$(document).on('click','#check_all',function(){$('.checkmail').click();return false;});if($('.checkmail').length>0){$('.checkmail').each(function(){$(this).on('click',function(){if($(this).closest('tr').hasClass('checked')){$(this).closest('tr').removeClass('checked');}else{$(this).closest('tr').addClass('checked');}});});}
$(document).on('click','.mail-important',function(){$(this).find('i.fa').toggleClass('fa-star').toggleClass('fa-star-o');});if($('#drop-zone').length>0){var dropZone=document.getElementById('drop-zone');var uploadForm=document.getElementById('js-upload-form');var startUpload=function(files){console.log(files);};uploadForm.addEventListener('submit',function(e){var uploadFiles=document.getElementById('js-upload-files').files;e.preventDefault();startUpload(uploadFiles);});dropZone.ondrop=function(e){e.preventDefault();this.className='upload-drop-zone';startUpload(e.dataTransfer.files);};dropZone.ondragover=function(){this.className='upload-drop-zone drop';return false;};dropZone.ondragleave=function(){this.className='upload-drop-zone';return false;};}
if(screen.width>=992){$(document).on('click','#toggle_btn',function(){if($('body').hasClass('mini-sidebar')){$('body').removeClass('mini-sidebar');$('.subdrop + ul').slideDown();}else{$('body').addClass('mini-sidebar');$('.subdrop + ul').slideUp();}
return false;});$(document).on('mouseover',function(e){e.stopPropagation();if($('body').hasClass('mini-sidebar')&&$('#toggle_btn').is(':visible')){var targ=$(e.target).closest('.sidebar').length;if(targ){$('body').addClass('expand-menu');$('.subdrop + ul').slideDown();}else{$('body').removeClass('expand-menu');$('.subdrop + ul').slideUp();}
return false;}});}
if($('[data-feather]').length>0){feather.replace();}
$('.app-listing .selectBox').on("click",function(){$(this).parent().find('#checkBoxes').fadeToggle();$(this).parent().parent().siblings().find('#checkBoxes').fadeOut();});$('.invoices-main-form .selectBox').on("click",function(){$(this).parent().find('#checkBoxes-one').fadeToggle();$(this).parent().parent().siblings().find('#checkBoxes-one').fadeOut();});$(function(){$("input[name='invoice']").click(function(){if($("#chkYes").is(":checked")){$("#show-invoices").show();}else{$("#show-invoices").hide();}});});$(".add-table-items").on('click','.remove-btn',function(){$(this).closest('.add-row').remove();return false;});if($('#editor').length>0){ClassicEditor.create(document.querySelector('#editor'),{toolbar:['bold','italic','link']}).then(editor=>{window.editor=editor;}).catch(err=>{console.error(err.stack);});}
$(document).on("click",".add-links1",function(){var experiencecontent='<div class="links-cont">'+
'<div class="service-amount">'+
'<a href="#" class="service-trash1"><i class="fa fa-minus-circle me-1"></i>Service Charge</a> <span>$ 4</span'+
'</div>'+
'</div>';$(".links-info-one").append(experiencecontent);return false;});$(".links-info-one").on('click','.service-trash1',function(){$(this).closest('.links-cont').remove();return false;});$(".links-info-discount").on('click','.service-trash-one',function(){$(this).closest('.links-cont-discount').remove();return false;});$(document).on("click",".logo-hide-btn",function(){$(this).parent().hide();});$(document).on("click",".add-btns",function(){var experiencecontent='<tr class="add-row">'+
'<td>'+
'<input type="text" class="form-control">'+
'</td>'+
'<td>'+
'<input type="text" class="form-control">'+
'</td>'+
'<td>'+
'<input type="text" class="form-control">'+
'</td>'+
'<td>'+
'<input type="text" class="form-control">'+
'</td>'+
'<td>'+
'<input type="text" class="form-control">'+
'</td>'+
'<td>'+
'<input type="text" class="form-control">'+
'</td>'+
'<td class="add-remove text-end">'+
' <a href="javascript:void(0);" class="add-btns me-2"><i class="fas fa-plus-circle"></i></a> '+
' <a href="#" class="copy-btn me-2"><i class="fas fa-copy"></i></a>'+
'<a href="javascript:void(0);" class="remove-btn"><i class="fa fa-trash-alt"></i></a>'+
'</td>'+
'</tr>';$(".add-table-items").append(experiencecontent);return false;});$(document).on("click",".add-links",function(){var experiencecontent='<div class="links-info"><div class="row form-row links-cont">'+
'<div class="input-block form-placeholder d-flex">'+
'<button class="btn social-icon"><i class="feather-github"></i></button>'+
'<input type="text" class="form-control" placeholder="Social Link">'+
'<a href="#" class="btn trash">'+
'<i class="feather-trash-2"></i>'+
'</a>'+
'</div>'+
'</div>'+
'</div>';$(".settings-form").append(experiencecontent);return false;});$(".settings-form").on('click','.trash',function(){$(this).closest('.links-cont').remove();return false;});});