
const SERVER_URL = "https://localhost:44343";
Vue.use(Toasted);
Vue.filter("currency", value => {
    return "$".concat(parseFloat(value).toFixed(2));
});
new Vue({
    el: "#app",
    data: () => ({
        all_orders: [],
    }),
    methods: {
        async getOrders() {
            const url = SERVER_URL + "/activity/getall/";
            const r = await fetch(url);
            const response = await r.json();
            this.all_orders = response;
        },
        edit(order) {
            window.location.href = "./edit.html?id=" + order.Activity_Id;
        },
    },
    async mounted() {
        await this.getOrders();
    }
});