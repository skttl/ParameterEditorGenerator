(function () {

	"use strict";

	function ParameterEditorGenerator($scope, $http, notificationsService, localizationService, appState, userService) {
		var vm = this;

		var dataTypeToGenerate = $scope.currentAction.metaData.dataTypeId;

		vm.mode = "";

		vm.result = {
			"Success": false,
			"PackageManifest": "",
			"FolderName": "",
			"Exists": false
		};


		$http.get("/umbraco/backoffice/ParameterEditorGenerator/Generator/IsDebugMode").then(function success(response) {
			vm.isDebug = response.data;
		});

		var generatePackageManifest = function (id, callback) {

			vm.loading = true;

			$http.get("/umbraco/backoffice/ParameterEditorGenerator/Generator/GetPackageManifestByDataTypeId/?datatypeId=" + id).then(function success(response) {

				vm.loading = false;

				vm.result.PackageManifest = JSON.stringify(response.data, null, 2);
				var folderName = response.data.propertyEditors[0].alias;
				folderName = folderName.substring(0, folderName.lastIndexOf("."));
				vm.result.FolderName = folderName;
				checkFolderExists(vm.result.FolderName);

			});
		}

		var checkFolderExists = function (folderName) {
			$http.get("/umbraco/backoffice/ParameterEditorGenerator/Generator/FolderExists/?folderName=" + folderName).then(function success(response) {

				vm.result.Exists = response.data;

			});
		}

		vm.checkFolderExists = function () {
			checkFolderExists(vm.result.FolderName);
		}


		vm.restart = function () {
			$http.post("/umbraco/backoffice/ParameterEditorGenerator/Generator/RestartAppPool").then(function (response) {
				window.location.reload();
			});
		}


		vm.savePackageManifest = function () {

			vm.loading = true;

			$http.post("/umbraco/backoffice/ParameterEditorGenerator/Generator/SavePackageManifest", { packageManifest: JSON.parse(vm.result.PackageManifest), folderName: vm.result.FolderName }).then(function success(response) {

				vm.loading = false;

				vm.result = response.data;

				if (!vm.result) {
					vm.mode = "error-saving-packagemanifest";
				}
				else {
					vm.mode = "successfully-saved-packagemanifest";
				}

			});
		}



		if (dataTypeToGenerate !== null) {
			generatePackageManifest(dataTypeToGenerate);
			vm.mode = "save-packagemanifest";
		}
		else {
			vm.mode = "no-datatypeid";
		}
	}

	angular.module("umbraco").controller("skttl.ParameterEditorGenerator.Controller", ParameterEditorGenerator);
})();