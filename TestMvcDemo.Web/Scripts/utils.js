//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURIComponent(r[2]); return null;
}

// 格式化时间
function dateFormat(fmt, date) {
    let ret;
    const opt = {
        "y+": date.getFullYear().toString(),        // 年
        "M+": (date.getMonth() + 1).toString(),     // 月
        "d+": date.getDate().toString(),            // 日
        "H+": date.getHours().toString(),           // 时
        "m+": date.getMinutes().toString(),         // 分
        "s+": date.getSeconds().toString()          // 秒
        // 有其他格式化字符需求可以继续添加，必须转化成字符串
    };
    for (let k in opt) {
        ret = new RegExp("(" + k + ")").exec(fmt);
        if (ret) {
            fmt = fmt.replace(ret[1], (ret[1].length == 1) ? (opt[k]) : (opt[k].padStart(ret[1].length, "0")))
        };
    };
    return fmt;
}

//返回消息
function getMessage(_this, resultCode) {
    let result = false;

    if (resultCode == 0) {
        _this.$message({
            message: '操作成功！',
            type: 'success'
        });

        result = true;
    } else if (resultCode == 10050) {
        _this.$message({
            message: '对不起，您没有权限！',
            type: 'warning'
        })
    } else if (resultCode == 10060) {
        _this.$message({
            message: '数据重复！',
            type: 'waring'
        })
    } else if (resultCode == 10040) {
        _this.$message.error("操作失败！");
    } else if (resultCode == 100006) {
        _this.$message.error("记录重复！");
    } else if (resultCode == 10070) {
        _this.$message.error("参数错误！");
    } else if (resultCode == 10200) {
        _this.$message.error("同步金牌失败！");
    } else if (resultCode == 10100) {
        _this.$message.error("此时间段已被预约，请重新打开订单详情选择预约时间！");
    } else if (resultCode == 10110) {
        _this.$message.error("该宝贝已拍摄过体验订单不能继续拍摄！");
    } else {
        _this.$message.error("操作失败！");
    }

    return result;
}

//返回消息
function getMobileMessage(_this, resultCode) {
    let result = false;

    if (resultCode == 0) {
        _this.$toast.success('操作成功！');
        result = true;
    } else if (resultCode == 10050) {
        _this.$toast('对不起，您没有权限！')
    } else if (resultCode == 10060) {
        _this.$toast('数据重复！')
    } else if (resultCode == 10040) {
        _this.$toast.fail("操作失败！");
    } else if (resultCode == 10070) {
        _this.$toast.fail("参数错误！");
    } else if (resultCode == 10200) {
        _this.$toast.fail("同步金牌失败！");
    } else if (resultCode == 10100) {
        _this.$toast.fail("此时间段已被预约，请重新打开订单详情选择预约时间！");
    } else if (resultCode == 10110) {
        _this.$toast.fail("该宝贝已拍摄过体验订单不能继续拍摄！");
    } else {
        _this.$toast.fail("操作失败！");
    }

    return result;
}

//是否包含字符串
function have(code, source) {
    var result = false;
    //source = "|" + source + "|";

    if (code.constructor == Array) {
        for (var i = 0; i < code.length; i++) {
            if (source.indexOf(code[i]) > -1)
                result = true;
        }
    } else if (code.constructor == String) {
        result = source.indexOf(code) > -1;
    }

    return result;
}

function convertdatetime(strTime, format) {
    var date = new Date(strTime);
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    var h = date.getHours();
    var mi = date.getMinutes();
    m = m < 10 ? "0" + m : m;
    d = d < 10 ? "0" + d : d;
    h = h < 10 ? "0" + h : h;
    mi = mi < 10 ? "0" + mi : mi;
    if (format == "date") {
        return y + "-" + m + "-" + d;
    } else if (format == "time") {
        return h + ":" + mi;
    } else {
        return y + "-" + m + "-" + d + " " + h + ":" + mi;
    }
}

//调整数组顺序
function swapArr(arr, index1, index2) {
    arr[index1] = arr.splice(index2, 1, arr[index1])[0];

    return arr;

}

//置顶
function toFirst(fieldData, index) {
    if (index != 0) {
        // fieldData[index] = fieldData.splice(0, 1, fieldData[index])[0]; 这种方法是与另一个元素交换了位子，

        fieldData.unshift(fieldData.splice(index, 1)[0]);

    }

}

//上移
function upGo(fieldData, index) {
    if (index != 0) {
        fieldData[index] = fieldData.splice(index - 1, 1, fieldData[index])[0];

    } else {
        fieldData.push(fieldData.shift());

    }

}

//下移
function downGo(fieldData, index) {
    if (index != fieldData.length - 1) {
        fieldData[index] = fieldData.splice(index + 1, 1, fieldData[index])[0];

    } else {
        fieldData.unshift(fieldData.splice(index, 1)[0]);

    }

}