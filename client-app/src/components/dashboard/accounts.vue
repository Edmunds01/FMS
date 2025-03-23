<script setup lang="ts">
var accounts = [{
    name: "KontName",
    balance: 100.1
},
{
    name: "KontName2",
    balance: 200.14
},
{
    name: "KontName3 really long name",
    balance: 300.15
}];

function handleClick(account: { name: string, balance: number }) {
    alert(account.name);
}

function createNewAccount() {
    alert("Create new account");
}

var total = accounts.reduce((acc, account) => acc + account.balance, 0);

</script>

<template>
    <div class="account-div">
        <div class="container">
            <div class="row border border-bottom-0">
                <div class="col text-center first-row full-center-text fs-1">Konti</div>
            </div>
            <div class="row border">
                <div class="col text-center">
                    <div>Kopā</div>
                    <div>{{ total.toEurFormat() }}</div>
                </div>
            </div>
            <div v-for="account in accounts" :key="account.name" class="row dashed-border">
                <button @click="handleClick(account)" class="col">
                    <div class="full-center-text text-ellipsis" :title="account.name"> {{ account.name }} </div>
                    <div class="full-center-text"> {{ account.balance.toEurFormat() }} </div>
                </button>
            </div>
            <div class="row border border-top-0">
                <button @click="createNewAccount()" class="col text-center">
                    <div >+ Pievienot</div>
                </button>
            </div>
        </div>
    </div>
</template>

<style>
:root {
    --bs-border-color: rgb(129, 127, 127);
    --bs-border-style: solid;
    --bs-border-width: 1.9px;
}
</style>

<style scoped>
.account-div {
    height: 100vh;
    width: 15vw;
}

.first-row {
    height: 5rem;
}

.full-center-text {
    display: flex;
    align-items: center;
    justify-content: center;
}

.dashed-border {
    border-bottom: var(--bs-border-width) dashed var(--bs-border-color);
    border-left: var(--bs-border-width) solid var(--bs-border-color);
    border-right: var(--bs-border-width) solid var(--bs-border-color);
}

.text-ellipsis {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    padding: 0 5px;
    text-align: left !important;
}
</style>
