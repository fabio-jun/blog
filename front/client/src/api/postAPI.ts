import api from "./axiosInstance";

export const getAllPosts = () => {
    return api.get("post");
};

export const getPostBySlug = (slug: string) => {
    return api.get(`post/${slug}`);
};

