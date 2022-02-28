
const SERVER_URL = "https://localhost:44343";
Vue.use(Toasted);
new Vue({
    el: "#app",
    data: () => ({
        activity: {
            Activity_Id: "",
            Activity_Status: "",
        },
    }),
    async mounted() {
        await this.getOrderDetails();
    },
    methods: {
        async getOrderDetails() {
            const urlSearchParams = new URLSearchParams(window.location.search);
            const id = urlSearchParams.get("id");
            const response = await fetch(`${SERVER_URL}/Activity/getbyid?id=${id}`);
            this.activity = await response.json();
            // this.activity = {
            //     Id: data.Activity_Id,
            //     Activity_Status: data.Activity_Status,
            // };
        },
        async save() {
            if (!this.activity.Activity_Status) {
                return this.$toasted.show("Please select status", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            const payload = JSON.stringify(this.activity);
            const url = SERVER_URL + "/Activity/processing/";
            const r = await fetch(url, {
                method: "PUT",
                body: payload,
                headers: {
                    "Content-type": "application/json",
                }
            });
            const response = await r.json();
            if (response) {
                window.location.href = "./get.html";
            } else {
                this.$toasted.show("Something went wrong. Try again", {
                    position: "top-left",
                    duration: 1000,
                });
            }
        }
    }
});