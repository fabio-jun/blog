import api from "./axiosInstance";

export const getAllPosts = () => {
    return api.get("post");
};

export const getPostBySlug = (slug: string) => {
    return api.get(`post/${slug}`);
};

export const createPost = (data: { title: string; content: string; isPublished: boolean }) => {
    return api.post("/post", data);
};

export const updatePost = (id: number, data: { title: string; content: string ; isPublished: boolean }) => {
    return api.put(`/post/${id}`, data);
};

export const deletePost = (id: number) => {
    return api.delete(`/post/${id}`);
}

