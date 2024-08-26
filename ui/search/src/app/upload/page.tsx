'use client'
import { FormEvent } from "react"

export default function Page(){
    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const formdata = new FormData(e.currentTarget)

        try{
            const reqOpt = {
                method: "POST",
                body: formdata,
            }
            let response = await fetch("http://localhost:5268/api/documents/upload", reqOpt)
            if (response.ok){
                alert("Uploaded successfully")
            }
            else{
                alert("Failed to upload. Try again later!")
            }
        } catch(err){
            console.error(err)
        }
    }
    return(
        <main className="">
            <form 
            onSubmit={handleSubmit}
            className="mx-auto my-32 p-4 max-w-[500px] bg-slate-800 rounded">
                <label htmlFor="file" className="block text-center font-bold hover:cursor-pointer pb-4">Select Document for Upload</label>
                <input id="file" type="file" name="file" className="hover:cursor-pointer block m-auto bg-slate-600 px-3 py-1"/>

                <button type="submit" className="block my-2 mx-auto py-2 px-5 rounded-md bg-white hover:bg-slate-600 hover:text-white text-black">Upload</button>
            </form>
        </main>
    )
}