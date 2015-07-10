'use strict';

(function() {

	/**
	*	Use when insert new Item into Menu that is between other items
	*/
	$.fn.insertAt = function (element, index) {
	    var lastIndex = this.children().size()
	    if (index < 0) {
	        index = Math.max(0, lastIndex + 1 + index)
	    }
	    this.append(element)
	    if (index < lastIndex) {
	        this.children().eq(index).before(this.children().last())
	    }
	    return this;
	};

	/**
	*	@author: spyhunter88
	*	Loop through all nodes in array and compare mappedID of child node 
	*  	with parentID of parentID to push it into its child
	*/
	Array.prototype.nestedArray = function (childName, parentID, mappedID) {
		// clone for later use
		var ref = [];
		// push into new array
		$.each(this, function(key, value) {
			ref.push(value);
		});

		// sort the array by parentID
		ref.sort(function(a,b) {
			var _parentIDa = a[parentID] || 0;
			var _parentIDb = b[parentID] || 0;
			return (a-b);
		});

		var result = [];
		// function to look up id in result
		function lookUpId(node, item) {
			var success = false;
			// determine
			if (node[parentID] == item[mappedID]) {
				if (node[childName] == undefined) node[childName] = [];
				node[childName].push(item);
				return true;
			}

			// lookup into each child
			if (node[childName] != undefined && node[childName].length > 0) {
				for (var i = 0, len = node[childName].length; i < len; i++) {
					success = success || lookUpId(node[childName][i], item);
				}
			}
			return success;
		};

		// push the first item in list
		if (ref.length > 0) {
			result.push(ref[0]);
			ref.splice(0,1);	// remove this
		}

		// Look up for parent or insert right into list
		while (ref.length > 0) {
			var node = ref[0];
			var success = false;
			for (var i = 0; i < result.length; i++) {
				success = success || lookUpId(result[i], node);
			}
			if (!success) {
				result.push(node);
			}

			// always remove this first node from list
			ref.splice(0,1);
		}

		return result;
	};

	/*
	*	Reserve into flatten array
	*/
	Array.prototype.flattenArray = function(childName) {
		var result = [];

		function retrieveDeep(node, parentArr) {
			var clone = $.extend({}, node);
			delete clone[childName];
			result.push(clone);

			if (node[childName] == undefined || node[childName].length == 0) {
				
				
			} else {
				for (var i = 0, len = node[childName].length; i < len; i++) {
					retrieveDeep(node[childName][i], node[childName]);
				}
			}
		};

		for (var i = 0, len = this.length; i < len; i++) {
			retrieveDeep(this[i], this);
		}

		return result;
	};
})();

