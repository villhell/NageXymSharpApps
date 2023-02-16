import { getActiveNode } from "symbol-node-util";
import { SSSWindwo, setTransactionByPayload, requestSign, getActiveAddress } from "sss-module";

export async function GetActiveNode(network) {
    return await getActiveNode(network);
}

export function SetTransactionByPayload(payload) {
    setTransactionByPayload(payload);
    return requestSign();
}

export function GetActiveAddress() {
    return getActiveAddress();
}
