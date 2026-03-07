import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getPostBySlug } from "../api/postApi";

interface Post {
    id: number;
    title: string;
    content: string;
    authorName: string;
    createdAt: string;
}

export default function PostPage() {
    const { slug } = useParams();
    const [post, setPost] = useState<Post | null>(null);

    useEffect(() => {
        if(slug) {
            getPostBySlug(slug).then((res) => setPost(res.data));
        }

    }, [slug]);

    if(!post) return <p>Carregando...</p>;

    return (
        <div>
            <h1>{post.title}</h1>
            <p>by {post.authorName} in {new Date(post.createdAt).toLocaleDateString()}</p>
            <div>{post.content}</div>
        </div>
    );
}