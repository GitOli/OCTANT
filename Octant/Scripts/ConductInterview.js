/**
 *  ConductInterview.js
 * --------- Description    : Customize CSS design for view & TreeView
 *                            Angular app to handle questions
 * --------- Date           : 12/06/15
 * --------- Author         : fl
 * --------- Requires       : jQuery, Angular
 * 
 */




/* Questions partial (angular) */

var conductinterview = angular.module('conductinterview', ['bootstrap-switch', 'jkuri.slimscroll']);

conductinterview.run(function ($window, $rootScope) {
    $rootScope.online = navigator.onLine;
    $window.addEventListener("offline", function () {
        $rootScope.$apply(function () {
            $rootScope.online = false;
        });
    }, false);
    $window.addEventListener("online", function () {
        $rootScope.$apply(function () {
            $rootScope.online = true;
        });
    }, false);
});

conductinterview.controller('conductinterviewCtrl', ['$scope', '$window', '$filter', 'qlist', 'qcomments', 'idinterview', function ($scope, $window, $filter, qlist, qcomments, idinterview) {
    /* Tree View */
    jQuery(document).ready(function () {
        function resizeTreeview () {
            $('#treeview > div > div').height($(window).height()); // - $('#treeview').offset().top + 100
            $('#treeview > div > div > div').height($(window).height());
        }
        $(window).load(resizeTreeview);
        $(window).resize(resizeTreeview);

        $('#clear').click(function () {
            //$('#configform')[0].reset();
            clear();
        });
    });

    $scope.skip = function (qaskip) {
        if ($scope.qaskip) { //If it is checked
            if (saveanswer()) {
                $scope.qn++;
                setNewQuest();
            }
        }
    }

    $scope.$watch("rolesModel", function () {
        $scope.selectedRoles = [];
        angular.forEach($scope.rolesModel, function (value, key) {
            if (value) {
                $scope.selectedRoles.push(parseInt(key.substr(2)));
            }
        });

        // Update $scope.qlist
        $scope.qcomments = $filter('filter')(qcomments, function (value, index) { return ($scope.selectedRoles.indexOf(value.Item2) > -1) });
        $scope.qlist = $filter('filter')(qlist, function (value, index) { return ($scope.selectedRoles.indexOf(value.Item6) > -1) });
        $scope.qlistlen = $scope.qlist.length - 1;
        $scope.qn = 0;
        setNewQuest();

        // Update tree
        $(document).ready(function () {
            $('.question').each(function () {
                var idquest = $(this).attr('ng-click').substring(12, $(this).attr('ng-click').length - 1);
                if ($scope.qlist.map(function (e) { return e.Item1; }).indexOf(parseInt(idquest)) > -1) {
                    $(this).removeClass('invisible');
                }
                else {
                    $(this).addClass('invisible');
                }
            });
        });

        // Update progression
        $(document).ready(function () {
            $('.node').each(function () {
                updateprog($(this));
            });
        });
    });

    // Roles
    $scope.selectedRoles = [];
    $scope.updateRoles = function () {
        // Update selected roles
        $scope.selectedRoles = [];
        angular.forEach($scope.rolesModel, function (value, key) {
            if (value) {
                $scope.selectedRoles.push(parseInt(key.substr(2)));
            }
        });

        // Update $scope.qlist
        $scope.qcomments = $filter('filter')(qcomments, function (value, index) { return ($scope.selectedRoles.indexOf(value.Item2) > -1) });
        $scope.qlist = $filter('filter')(qlist, function (value, index) { return ($scope.selectedRoles.indexOf(value.Item6) > -1) });
        $scope.qlistlen = $scope.qlist.length - 1;
        $scope.qn = 0;
        setNewQuest();

        // Update tree
        $(document).ready(function () {
            $('.question').each(function () {
                var idquest = $(this).attr('ng-click').substring(12, $(this).attr('ng-click').length - 1);
                if ($scope.qlist.map(function (e) { return e.Item1; }).indexOf(parseInt(idquest)) > -1) {
                    $(this).removeClass('invisible');
                }
                else {
                    $(this).addClass('invisible');
                }
            });
        });

        // Update progression
        $(document).ready(function () {
            $('.node').each(function () {
                updateprog($(this));
            });
        });
    };

    // Questions
    $scope.qname = "No question";
    $scope.qdescription = "No question";
    $scope.qabool = {
        val: 'No'
    };
    $scope.qachk = {
        chk1: false,
        chk2: false,
        chk3: false,
        chk4: false,
        chk5: false
    };
    $scope.qarad = {
        val: ''
    };
    $scope.qaskip = false;

    $scope.updateRoles();

    $scope.setQuestion = function (idquest) {
        //$(".question").css("background-color", "white");
        //$(this).css("background-color", "red");
        if (saveanswer()) {
            var index = $scope.qlist.map(function (e) { return e.Item1; }).indexOf(idquest);
            $scope.qn = index;
            setNewQuest();
        }
        
    };

    $scope.qprev = function () {
        if (saveanswer()) {
            $scope.qn--;
            setNewQuest();
        }
    }
    $scope.qnext = function () {
        if (saveanswer()) {
            $scope.qn++;
            setNewQuest();
        }
    }

    function clear() {
        $scope.qaint = undefined;
        $scope.qabool = {
            val: 'No'
        };
        $scope.qachk = {
            chk1: false,
            chk2: false,
            chk3: false,
            chk4: false,
            chk5: false
        };
        $scope.qarad = {
            val: ''
        };
        $scope.qaskip = false;
        $scope.qacomment = '';
        $scope.qaconsultantcomment = '';

        saveanswer();
    }

    function setNewQuest() {
        if ($scope.qlist.length == 0) {
            $scope.qname = "No question";
            $scope.qdescription = "No question";
            $('#divtextarea').css('display', 'none');
            $('#divint').css('display', 'none');
            $('#divbool').css('display', 'none');
            for (var k = 1; k <= 5; k++) {
                $('#chk' + k).css('display', 'none');
                $('#rad' + k).css('display', 'none');
            }
            $('#divcomment').css('display', 'none');
            $('#divconsultantcomment').css('display', 'none');
            $('#divskip').css('display', 'none');
            return;
        }
        if ($scope.qn < 0) {
            $scope.qn = 0;
        }
        if ($scope.qn > $scope.qlistlen) {
            $scope.qn--;
        }
        if ($scope.qlistlen < 0) {
            return;
        }

        $scope.qname = $scope.qlist[$scope.qn].Item2;
        $scope.qdescription = $scope.qlist[$scope.qn].Item3;

        $('.question.active').css("backgroundColor", "transparent").removeClass('active');
        var actual = $scope.qlist[$scope.qn].Item1;
        $('.question[ng-click="setQuestion(' + actual + ')"]').addClass('active').css("backgroundColor", "lightblue");

        $('#divtextarea').css('display', 'none');
        $('#divint').css('display', 'none');
        $('#divbool').css('display', 'none');
        for (var k = 1; k <= 5; k++) {
            $('#chk' + k).css('display', 'none');
            $('#rad' + k).css('display', 'none');
        }

        if ($scope.qlist[$scope.qn].Item4 != null) {
            switch ($scope.qlist[$scope.qn].Item4.toString()) {
                case 'Text': $('#divtextarea').css('display', 'block'); break;
                case 'Integer': $('#divint').css('display', 'block'); break;
                case 'Boolean': $('#divbool').css('display', 'block'); break;
                case 'Real': break;
                default: choice($scope.qlist[$scope.qn].Item4.toString()); break;
            }
        }
        $('#divcomment').css('display', 'block');
        $('#divconsultantcomment').css('display', 'block');
        $('#divskip').css('display', 'block');

        loadanswer();
    };

    function choice(answer) {
        var arrayAnswers = answer.split(";");

        if (arrayAnswers.length > 0) {
            for (var i = 0; i < arrayAnswers.length; i++) {
                var obj = arrayAnswers[i];
                var j = i + 1;

                if ($scope.qlist[$scope.qn].Item7) {
                    $('#chk' + j + ' span').html(obj);
                    $('#chk' + j).css('display', 'block');
                }
                else {
                    $('#rad' + j + ' span').html(obj);
                    $('#rad' + j).css('display', 'block');
                }
            }
        }
    };

    function saveanswer() {
        if ($scope.qlist[$scope.qn].Item4 != null) {
            switch ($scope.qlist[$scope.qn].Item4.toString()) {
                case 'Text': $scope.qlist[$scope.qn].Item5 = $scope.qatext; break;
                case 'Integer': $scope.qlist[$scope.qn].Item5 = $scope.qaint; break;
                case 'Boolean': $scope.qlist[$scope.qn].Item5 = $scope.qabool.val; break;
                case 'Real': break;
                default:
                    if ($scope.qlist[$scope.qn].Item7) {
                        var tosave = "";
                        tosave += $scope.qachk.chk1 + ';';
                        tosave += $scope.qachk.chk2 + ';';
                        tosave += $scope.qachk.chk3 + ';';
                        tosave += $scope.qachk.chk4 + ';';
                        tosave += $scope.qachk.chk5;
                        $scope.qlist[$scope.qn].Item5 = tosave;
                    }
                    else {
                        $scope.qlist[$scope.qn].Item5 = $scope.qarad.val;
                    }
                    break;
            }
            $scope.qcomments[$scope.qn].Item3 = $scope.qacomment;
            $scope.qcomments[$scope.qn].Item4 = $scope.qaconsultantcomment;
            $scope.qcomments[$scope.qn].Item5 = !$scope.qaskip;
            var actual = $scope.qlist[$scope.qn].Item1;
            if (($scope.qcomments[$scope.qn].Item5 != undefined) && ($scope.qcomments[$scope.qn].Item5)) {
                $('.question[ng-click="setQuestion(' + actual + ')"]').find('.question-validate i.ion-ios-close-empty').css('display', 'none');
                if (($scope.qlist[$scope.qn].Item5 != undefined) && (($scope.qlist[$scope.qn].Item5 !== ""))) {
                    $('.question[ng-click="setQuestion(' + actual + ')"]').find('.question-validate i.ion-ios-checkmark-empty').css('display', 'block');
                    updateprog($('.question[ng-click="setQuestion(' + actual + ')"]').prevAll('.node').first());
                }
                else {
                    $('.question[ng-click="setQuestion(' + actual + ')"]').find('.question-validate i.ion-ios-checkmark-empty').css('display', 'none');
                    updateprog($('.question[ng-click="setQuestion(' + actual + ')"]').prevAll('.node').first());
                }
            }
            else {
                $('.question[ng-click="setQuestion(' + actual + ')"]').find('.question-validate i.ion-ios-checkmark-empty').css('display', 'none');
                $('.question[ng-click="setQuestion(' + actual + ')"]').find('.question-validate i.ion-ios-close-empty').css('display', 'block');
                updateprog($('.question[ng-click="setQuestion(' + actual + ')"]').prevAll('.node').first());
            }
        }

        // Validation on input number
        if ($scope.qlist[$scope.qn].Item4 != null) {
            if ($scope.qlist[$scope.qn].Item4.toString() == 'Integer') {
                if ($scope.form.qaint.$invalid) {
                    return false;
                }
            }
        }

        return true;
    };

    function updateprog(currentnode) {
        var i = 0;
        var n = 0;
        var t = 0;
        if (currentnode.hasClass("node1")) {
            currentnode.nextUntil('.node1').not('.invisible').each(function () {
                if ($(this).find('.ion-ios-close-empty').css('display') == 'none')
                    n++;
                if ($(this).find('.ion-ios-checkmark-empty').css('display') == 'block')
                    i++;
                t++;
            });
        } else {
            currentnode.nextUntil('.node').not('.invisible').each(function() {
                if ($(this).find('.ion-ios-close-empty').css('display') == 'none')
                    n++;
                t++;
            });
            currentnode.nextUntil('.node').not('.invisible').each(function() {
                if ($(this).find('.ion-ios-checkmark-empty').css('display') == 'block')
                    i++;
            });
        }
        if (t == 0) {
            currentnode.addClass('invisible');
            //var p = 100;
            var p = 0;
        }
        else {
            currentnode.removeClass('invisible');
            var p = Math.round((i / n) * 100);
            if (n == 0) {
                var p = 100;
            }
        }

        currentnode.find('.node-progression').html(p + "%");
        if (p >= 0) {
            currentnode.find('.node-progression').addClass('text-danger').removeClass('text-warning').removeClass('text-info').removeClass('text-success');
        }
        if (p >= 25) {
            currentnode.find('.node-progression').removeClass('text-danger').addClass('text-warning').removeClass('text-info').removeClass('text-success');
        }
        if (p >= 50) {
            currentnode.find('.node-progression').removeClass('text-danger').removeClass('text-warning').addClass('text-info').removeClass('text-success');
        }
        if (p >= 100) {
            currentnode.find('.node-progression').removeClass('text-danger').removeClass('text-warning').removeClass('text-info').addClass('text-success');
        }

        /*if (currentnode.parent().parent().prev().is('span')) {
            updateprog(currentnode.parent().parent().prev());
        }*/
    };

    function loadanswer() {
        if ($scope.qlist[$scope.qn].Item4 != null) {
            switch ($scope.qlist[$scope.qn].Item4.toString()) {
                case 'Text': $scope.qatext = $scope.qlist[$scope.qn].Item5; break;
                case 'Integer':
                    if (($scope.qlist[$scope.qn].Item5 == null) || ($scope.qlist[$scope.qn].Item5 === ""))
                        $scope.qaint = undefined;
                    else
                        $scope.qaint = parseInt($scope.qlist[$scope.qn].Item5);
                    break;
                case 'Boolean':
                    if (($scope.qlist[$scope.qn].Item5 != null) && ($scope.qlist[$scope.qn].Item5 !== ""))
                        $scope.qabool.val = $scope.qlist[$scope.qn].Item5;
                    else
                        $scope.qabool.val = 'No';
                    break;
                case 'Real': break;
                default:
                    $scope.qachk.chk1 = false;
                    $scope.qachk.chk2 = false;
                    $scope.qachk.chk3 = false;
                    $scope.qachk.chk4 = false;
                    $scope.qachk.chk5 = false;
                    $scope.qarad = { val: '' };
                    if ($scope.qlist[$scope.qn].Item7) {
                        if ($scope.qlist[$scope.qn].Item5 && ($scope.qlist[$scope.qn].Item5 != "")) {
                            var arrayAnswers = $scope.qlist[$scope.qn].Item5.split(";");
                            if (arrayAnswers.length >= 1) {
                                $scope.qachk.chk1 = (arrayAnswers[0] == "true");
                            }
                            if (arrayAnswers.length >= 2) {
                                $scope.qachk.chk2 = (arrayAnswers[1] == "true");
                            }
                            if (arrayAnswers.length >= 3) {
                                $scope.qachk.chk3 = (arrayAnswers[2] == "true");
                            }
                            if (arrayAnswers.length >= 4) {
                                $scope.qachk.chk4 = (arrayAnswers[3] == "true");
                            }
                            if (arrayAnswers.length >= 5) {
                                $scope.qachk.chk5 = (arrayAnswers[4] == "true");
                            }
                        }
                    }
                    else {
                        if ($scope.qlist[$scope.qn].Item5 != null)
                            $scope.qarad = { val: $scope.qlist[$scope.qn].Item5 };
                    }
                    break;
            }
            $scope.qacomment = $scope.qcomments[$scope.qn].Item3;
            $scope.qaconsultantcomment = $scope.qcomments[$scope.qn].Item4;
            $scope.qaskip = !$scope.qcomments[$scope.qn].Item5;
        }
        else {
            $scope.qatext = undefined;
            $scope.qaint = undefined;
            $scope.qabool = {
                val: 'No'
            };
            $scope.qachk = {
                chk1: false,
                chk2: false,
                chk3: false,
                chk4: false,
                chk5: false
            };
            $scope.qarad = { val: '' };
            $scope.qacomment = undefined;
            $scope.qaconsultantcomment = undefined;
            $scope.qaskip = false;
        }
    };

    $scope.tvdisplayed = true;
    $scope.tvdisplay = function () {
        if ($scope.tvdisplayed) {
            $("#sidebar-wrapper").css("display", "none");
            $("#tvdisplay").removeClass("col-md-3").addClass("col-md-1");
            $("#question-container").removeClass("col-md-7").addClass("col-md-9");
            $(".treeview-hide").css("display", "block");
            $scope.tvdisplayed = false;
        }
        else {
            $("#question-container").removeClass("col-md-9").addClass("col-md-7");
            $("#tvdisplay").removeClass("col-md-1").addClass("col-md-3");
            $(".treeview-hide").css("display", "none");
            $("#sidebar-wrapper").css("display", "block");
            $scope.tvdisplayed = true;
        }
    };

    $scope.$watch('online', function (newStatus) {
        if (newStatus)
            $scope.onlinestatus = "ON";
        else
            $scope.onlinestatus = "OFF";
    });

    $scope.cisave = function () {
        //updateCompletion();
        if ($scope.qlistlen > -1) {
            saveanswer();
        }
        var qlisttosave = qlist;
        qlisttosave.forEach(function (entry) {
            delete entry["Item2"];
            delete entry["Item3"];
            delete entry["Item4"];
            delete entry["Item6"];
            delete entry["Item7"];
        });
        if ($scope.onlinestatus) {
            // Access internet ON
            // Envoyer qlist vers code ASP.NET pour synchro avec la base 
            var DTO = JSON.stringify({ 'tosave': qlisttosave, 'idinterview': idinterview, 'comments': qcomments });
            $.ajax({
                url: '/ConductInterview/Save',
                cache: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: DTO,
                dataType: "text",
                success: function (data, status) {
                    if (data == "0") {
                        $scope.issaved = false;
                        alert('Save failed : empty interview');
                    }
                    else {
                        $scope.issaved = true;
                        window.location.href = '/ConductInterview/Saved/' + data;
                    }
                },
                error: function (data, status, err) {
                    $scope.issaved = false;
                    alert('Save failed : ' + err.toString());
                }
            });
        }
        else {
            // Access internet OFF
            // Sauvegarder qlist dans localDb (+ synchro avec bdd online lorsque connexion OK) ?
            alert('Save failed : you are not connected');
        }
    };

    $scope.issaved = false;
    // Alert de confirmation avant de quitter la page
    $window.addEventListener("beforeunload", function (e) {
        var confirmationMessage = "Caution : the interview is not saved";

        if (!$scope.issaved) {
            (e || window.event).returnValue = confirmationMessage;
            return confirmationMessage;
        }
    });

}]);
