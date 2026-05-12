import { createEntityAdapter, createSlice } from "@reduxjs/toolkit";
import { ActivityEvent } from "../../types/ActivityEvent";

const activitiesAdapter = createEntityAdapter<ActivityEvent>({
  sortComparer: (a, b) => b.timestamp.localeCompare(a.timestamp),
});

const activitiesSlice = createSlice({
  name: "activities",
  initialState: activitiesAdapter.getInitialState(),
  reducers: {
    setActivities: activitiesAdapter.setAll,
    addActivity: activitiesAdapter.addOne,
    updateActivityCategory: activitiesAdapter.updateOne,
  },
});

export const { setActivities, addActivity, updateActivityCategory } =
  activitiesSlice.actions;
export const {
  selectAll: selectAllActivities,
  selectById: selectActivityById,
} = activitiesAdapter.getSelectors(
  (state: { activities: ReturnType<typeof activitiesSlice.getInitialState> }) =>
    state.activities,
);

export default activitiesSlice.reducer;
