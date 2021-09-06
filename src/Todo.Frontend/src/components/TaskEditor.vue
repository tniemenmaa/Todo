<template>
    <b-form @submit="onChange">
        <b-form-group label="Summary" :label-for="'input-'+task.id+'-summary'">
            <b-form-input @change="onChange" :id="'input-'+task.id+'-summary'" v-model="task.summary" :state="summaryState" type="text"></b-form-input>
            <b-form-invalid-feedback>
                <template v-if="task.summary.length > config.validation.maxSummary">
                    Max summary length is {{ config.validation.maxSummary }} characters.
                </template>
            </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group label="Description" :label-for="'input-'+task.id+'-description'">
            <b-form-textarea @change="onChange" :id="'input-'+task.id+'-description'" v-model="task.description" :state="descriptionState" placeholder="Description for the task."></b-form-textarea>
                    <b-form-invalid-feedback>
                <template v-if="task.description.length > config.validation.maxDescription">
                    Max description length is {{ config.validation.maxDescription }} characters.
                </template>
            </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group label="Deadline" :label-for="'input-'+task.id+'-deadline'">
            <b-form-datepicker @change="onChange" :id="'input-'+task.id+'-deadline'" v-model="task.deadlineAt" :state="deadlineState" today-button  reset-button close-button></b-form-datepicker>
                    <b-form-invalid-feedback>
                <template v-if="task.description.length > config.validation.maxDescription">
                    Invalid deadline.
                </template>
            </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group label="Priority" :label-for="'input-'+task.id+'-priority'">
            <b-form-input @change="onChange" :id="'input-'+task.id+'-priority'" :max="Number.MAX_SAFE_INTEGER" :min="Number.MIN_SAFE_INTEGER" v-model.number="task.priority" :state="priorityState" type="number"></b-form-input>
            <b-form-invalid-feedback>
                <template>
                    Priority should be a numeric value between {{ Number.MIN_SAFE_INTEGER }} and {{ Number.MAX_SAFE_INTEGER }}.
                </template>
            </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group label="Status" :label-for="'input-'+task.id+'-status'">
            <b-form-select @change="onChange" :id="'input-'+task.id+'-status'" v-model="task.status" :options="config.taskStates"></b-form-select>
            <b-form-invalid-feedback>
                <template>
                    Priority should be a numeric value between {{ Number.MIN_SAFE_INTEGER }} and {{ Number.MAX_SAFE_INTEGER }}.
                </template>
            </b-form-invalid-feedback>
        </b-form-group>
    </b-form>
</template>
<script lang="js">
import Vue from 'vue';
import axios from 'axios';

export default Vue.extend({
    name: 'task-editor',
    data() {
        return {
            config: {
                validation: {
                    maxSummary: 128,
                    maxDescription: 1024
                },
                taskStates: [
                    { value: 0, text: '' },
                    { value: 1, text: 'Reserved' },
                    { value: 2, text: 'Ongoing' },
                    { value: 3, text: 'Done' },
                    { value: 4, text: 'Pending' }
                ]
            }
        }
    },
    props: {
        task: Object,
        tasks: Array,
        autosave: Boolean,
        parentId: { type: Number, default: null }
    },
    computed: {
        summaryState() {
            if ( !this.task.summary ) return null;
            if ( this.task.summary.length > this.config.validation.maxSummary ) return false;
            return true;
        },
        descriptionState() {
            if ( !this.task.description ) return null;
            if ( this.task.description.length > this.config.validation.maxDescription ) return false;
            return true;
        },
        deadlineState() {
            if ( !this.task.deadlineAt ) return null;
            if ( isNaN(Date.parse(this.task.deadlineAt)) ) return false;
            return true;
        },
        priorityState() {
            if ( typeof this.task.priority !== "number" ) return false;
            if ( !this.task.priority ) return null;
            if ( !Number.isInteger(this.task.priority) ) return false;
            if ( !Number.isSafeInteger(this.task.priority) ) return false;
            return true;
        }
    },
    methods: {
        // Input change
        onChange() {
            if (this.autosave) {
                this.save();
            }
        },
        save() {
            this.$emit("save", data);
        }
    }
});
</script>