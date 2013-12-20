$(document).ready(function () {
    var modelJson = {
        UserOneId: "11",
        UserTwoId: "13",
        ActiveUserId: "11",
        Board : [
            [1, 2, 0],
            [0, 1, 0]
        ]
    };
    
    console.log("Tests");
    console.log($);
    
    function BoardViewModel() {
        var self = this;
        self.UserOneId = ko.observable(1);
        self.UserTwoId = ko.observable(2);
        self.ActiveUserId = ko.observable(3);
        self.Board = ko.observableArray();

        self.init = function () {
            //ko.mapping.fromJS(viewModelJson.json, {}, self.Board);
            //ko.mapping.fromJS(viewModelJson.json.Board, {}, self.Board);
            //self.initialized(true);
        };
        
        self.ActiveCellType = ko.computed(function() {
            var activeCellType = "Empty";
            if (self.ActiveUserId() == self.UserOneId()) {
                activeCellType = "Black";
            } else {
                activeCellType = "White";
            }
                
            return activeCellType;
        });

        self.GetClassForCell = function (i, j) {
            //var res = self.Board();
            //console.log(res);
            //var res = self.Board(0);
            //console.log(self.Board(0));
            //var res = self.Board(0, 0);
            //console.log(self.Board(0, 0));
            //var res = self.Board([0,0]);
            //console.log(self.Board([0, 0]));

            var typeValue = self.Board[i, j];
            return "cell-type-" + self.GetCellTypeText(typeValue);
        };
        
        self.GetCellTypeText = function (cellTypeId) {
            var typeText = "empty";
            if (cellTypeId == 1) {
                typeText = "black";
            }
            if (cellTypeId == 2) {
                typeText = "white";
            }
            return typeText;
        };
    }

    var viewModel = new BoardViewModel();
    ko.mapping.fromJS(modelJson, {}, viewModel);
    ko.applyBindings(viewModel);
});

