﻿(
    function () {
        angular.module("noticeManagement_module").controller("viewNotice_controller", ["$scope", "Notice_Service", "Utility_Service", "$window", function ($scope, NoticeService, UtilityService, $window) {

           

            $scope.selected = {
                program: {
                },
                semester: {
                },
                file:{
                }
                
            }

            $scope.selection = {
                programs: [],
                semesters: [],
                files:[]
            }

            $scope.message = {
                content: "",
                color: ""
            }


            $scope.whenProgramIsSelected = function () {
                getSemesters();
                
                $scope.selected.semester = {};
                $scope.selected.file = {};

                $scope.selection.files = [];  
            }

            $scope.whenSemesterIsSelected = function () {
                getNotices();
            }

            $scope.viewNoticeFile = function (filePath) {
                var completeFilePath = 'ViewNotice?filePath=' + filePath;
                $window.open(completeFilePath, "_blank");
            }

            $scope.downloadNotice=function(filePath) {
                window.location = '/NoticeManagement/DownloadNotice?fileName=' + filePath;
            }


            var showNotification = function (message, status) {
                $scope.message.content = message;

                if (status == "OK") {
                    $scope.message.color = "alert alert-success";
                }
                else if (status == "ERROR") {
                    $scope.message.color = "alert alert-danger";
                }
                setTimeout(function () {
                    $scope.$apply(function () {
                        $scope.message = {
                            content: "",
                            color: ""
                        }
                    });
                }, 3000);
            }

            var getPrograms = function () {
                UtilityService.getAllPrograms()
                   .then(
                          function (d) {
                              if (d.Status == "OK") {
                                  $scope.selection.programs = d.Data.programs;
                              }
                              else if (d.Status == "ERROR") {
                                  showNotification(d.Message, d.Status);
                              }
                          },
                           function (errResponse) {
                               showNotification('Error While Fetching Programs', 'ERROR');
                           }
                   );
            }

            var getSemesters = function () {
                UtilityService.getSemestersOfAProgram($scope.selected.program.id)
                   .then(
                          function (d) {
                              if (d.Status == "OK") {
                                  $scope.selection.semesters = d.Data.semesters;
                              }
                              else if (d.Status == "ERROR") {
                                  showNotification(d.Message, d.Status);
                              }
                          },
                           function (errResponse) {
                               showNotification('Error While Fetching Semesters', 'ERROR');
                           }
                   );
            }

            var getNotices = function () {
                NoticeService.getNotices_tp($scope.selected.program.id, $scope.selected.semester.id)
                   .then(
                          function (d) {
                              if (d.Status == "OK") {
                                  $scope.selection.files = d.Data;
                              }
                                  else if (d.Status == "ERROR") {
                                  showNotification(d.Message, d.Status);
                              }
                              },
                           function (errResponse) {
                               showNotification('Error While Fetching Semesters', 'ERROR');
                               }
                   );
                               }
            

            function clearDate() {
                var InformationToSave = {
                    title: "",
                    description: "",
                    programId: "",
                    semesterId: "",
                    batchId: ""
                }

                $scope.selected = {
                    program: {
                    },
                    semester: {
                    },
                    batch: {
                    },
                    file: {
                        title: "",
                        description: ""
                    }
                }

                $scope.selection = {
                    programs: $scope.selection.programs,
                    semesters: []
                }

                $scope.flag = {
                    disableSubmitButton: true
                }
            }

            var inatializeThePage = function () {

                getPrograms();

            }


            inatializeThePage();
        }])
    }()
)



