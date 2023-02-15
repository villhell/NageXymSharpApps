const nodeUtil = require("symbol-node-util");

export const getNode = async (network) => {
    return await getActiveNode(network);
}