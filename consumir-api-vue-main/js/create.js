
const SERVER_URL = "https://localhost:44343";
Vue.use(Toasted);
new Vue({
    el: "#app",
    data: () => ({
        activity: {
            Id: "",
            Activity_Title: "",
            Activity_Product: "",
            Activity_Status: "",
        },
    }),
    methods: {
        async save() {
            if (!this.activity.Id) {
                return this.$toasted.show("Please write Id", {
                    position: "top-left",
                    duration: 1000,
                });
            }

            if (!this.activity.Activity_Title) {
                return this.$toasted.show("Please write title", {
                    position: "top-left",
                    duration: 1000,
                });
            }

            if (!this.activity.Activity_Product) {
                return this.$toasted.show("Please write name product", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            if (!this.activity.Activity_Status) {
                return this.$toasted.show("Please select status", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            const payload = JSON.stringify(this.activity);
            const url = SERVER_URL + "/activity/add/";
            const r = await fetch(url, {
                method: "POST",
                body: payload,
                headers: {
                    Accept: 'application/json',
                    "Content-type": "application/json",
                }
            });
            const response = await r.json();
            if (response) {
                this.$toasted.show("New Order Saved", {
                    position: "top-left",
                    duration: 1000,
                });
                this.activity = {
                    Id: "0",
                    Activity_Title: "",
                    Activity_Status: "",
                    Activity_Status: "",
                };
            } else {
                this.$toasted.show("Something went wrong. Try again", {
                    position: "top-left",
                    duration: 1000,
                });
            }
        }
    }
});